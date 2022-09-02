#include "WiFiAP.h"

int valorSensor = 0;

AsyncWebServer server80(80); //Comandos(Verificar WiFi e Enviar valor de Sensores pro aplicativo C#)
AsyncWebServer server7(7);   //Controle Programavel por POST
AsyncWebServer server18(18); //Controle Unitario por parametros GET

IPAddress staticIP(10, 10, 10, 10);
IPAddress gateway(10, 10, 10, 1);
IPAddress subnet(255, 255, 255, 0);

WiFiAP::WiFiAP(char *ssid, char *password)
{
    _ssid = ssid;
    _password = password;
}

void WiFiAP::wifiStart()
{
    WiFi.softAP(_ssid, _password);
    WiFi.softAPConfig(staticIP, gateway, subnet);

    /********* SERVIDOR 80 ************************************************************************************************************************************/
    //Receba dados do ESP32 no body do HTTP chamando a route "/sensor" pelo metodo GET
    server80.on("/sensor", HTTP_GET, [&](AsyncWebServerRequest *request) {
        request->send(200, "text/plain", String(valorSensor)); //application/json -> POST e text/plain -> GET
    });

    server80.on("/", HTTP_GET, [&](AsyncWebServerRequest *request) { //Envia "OK" e recebe "200"
        int paramsNr = request->params();
        for (int i = 0; i < paramsNr; i++)
        {
            AsyncWebParameter *p = request->getParam(i);
            if (p->name() == "param")
            {
                conexaoAplicativoOK = p->value().toInt(); //recebendo "200"
            }
        }
        request->send(200); //enviando "OK"
    });

    server80.on("/readytosend/", HTTP_GET, [&](AsyncWebServerRequest *request) { //recebe "200"
        int paramsNr = request->params();
        for (int i = 0; i < paramsNr; i++)
        {
            AsyncWebParameter *p = request->getParam(i);
            if (p->name() == "param")
            {
                readyToSend = p->value().toInt(); //recebendo "200"
            }
        }
        request->send(200); //enviando "OK", mas não é lido pelo app C#
    });

    //Quando não encontra retorna erro pela porta 80
    server80.onNotFound([&](AsyncWebServerRequest *request) {
        request->send(404);
    });
    /***********************************************************************************************************************************************************/

    /********* SERVIDOR 7 **************************************************************************************************************************************/
    /*//Envie mensagens no body do HTTP pela route "/data" e a mensagem será escrita no monitor serial, se der certo retorna "Funcionou"  ---> Metodo POST
    server7.on("/data", HTTP_POST, [&](AsyncWebServerRequest *request) {}, NULL, [](AsyncWebServerRequest *request, uint8_t *data, size_t len, size_t index, size_t total) {
            for (size_t i = 0; i < len; i++)
            {
                Serial.write(data[i]);
            }
            Serial.println();
            request->send(200); });

    server7.onNotFound([&](AsyncWebServerRequest *request) {
        request->send(404);
    });*/
    /***********************************************************************************************************************************************************/

    /********* SERVIDOR 18 *************************************************************************************************************************************/
    //Para enviar parametros por GET, exemplo: HTTP://IP:18/?param1=10&paramx=hello
    server18.on("/", HTTP_GET, [&](AsyncWebServerRequest *request) {
        int paramsNr = request->params(); //numero de parametros recebidos
        for (int i = 0; i < paramsNr; i++)
        {
            AsyncWebParameter *p = request->getParam(i);
            //Serial.println(p->name());
            if (p->name() == "param1")
            {
                valorEixo1AP = p->value().toInt();
            }
            else if (p->name() == "param2")
            {
                valorEixo2AP = p->value().toInt();
            }
            else if (p->name() == "param3")
            {
                valorEixo3AP = p->value().toInt();
            }
            else if (p->name() == "param4")
            {
                valorEixo4AP = p->value().toInt();
            }
            else if (p->name() == "param5")
            {
                valorGarraAP = p->value().toInt();
            }
            else if (p->name() == "param6")
            {
                frameSpeed = p->value().toInt();
            }
        }
        request->send(200);
    });

    server18.onNotFound([&](AsyncWebServerRequest *request) {
        request->send(404);
    });
    /************************************************************************************************************************************************************/

    server80.begin();
    //server7.begin();
    server18.begin();
}

void WiFiAP::wifiReadOn18(uint16_t *leiturasWiFiAP18, uint16_t pot1, uint16_t pot2, uint16_t pot3, uint16_t pot4, uint16_t pot5)
{
    uint16_t read[6] = {valorEixo1AP, valorEixo2AP, valorEixo3AP, valorEixo4AP, valorGarraAP, frameSpeed};
    for (int i = 0; i < 6; i++)
    {
        if (readyToSend == 200 && conexaoAplicativoOK == 200) //200, codigo recebido pelo app
        {
            leiturasWiFiAP18[i] = read[i]; //leituras[i] === *(leituras + i)
        }
    }
    if (readyToSend != 200)
    {
        leiturasWiFiAP18[0] = pot1;
        leiturasWiFiAP18[1] = pot2;
        leiturasWiFiAP18[2] = pot3;
        leiturasWiFiAP18[3] = pot4;
        leiturasWiFiAP18[4] = pot5;
    }
}

void WiFiAP::wifiStop()
{
    WiFi.softAPdisconnect(true);
    WiFi.mode(WIFI_OFF);
}