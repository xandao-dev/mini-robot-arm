#ifndef WiFiAP_h
#define WiFiAP_h

#include "Arduino.h"
#include "WiFi.h"
#include <ESPAsyncWebServer.h>
#include <ArduinoJson.h>

class WiFiAP
{
public:
  WiFiAP(char *ssid, char *password);
  void wifiStart();
  void wifiReadOn18(uint16_t *leiturasWiFiAP18, uint16_t pot1, uint16_t pot2, uint16_t pot3, uint16_t pot4, uint16_t pot5);
  void wifiStop();

private:
  char *_ssid;
  char *_password;
  uint8_t conexaoAplicativoOK;
  uint8_t readyToSend;
  uint16_t valorEixo1AP;
  uint16_t valorEixo2AP;
  uint16_t valorEixo3AP;
  uint16_t valorEixo4AP;
  uint16_t valorGarraAP;
  uint8_t frameSpeed;
};

#endif
