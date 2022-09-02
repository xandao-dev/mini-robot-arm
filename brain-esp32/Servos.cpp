/*----------------------------------------------------------------------------------------
Essa biblioteca depende da biblioteca ESP32-Arduino-Servo-Library, criei essa biblioteca 
para deixar o arquivo principal(.ino) mais limpo.
-----------------------------------------------------------------------------------------*/

#include "Servos.h"

int servo_Delay = 20; //MG995 -> 20ms

int pos1Eixo1 = 0;
int posEixo1 = 0;
int pos1Eixo2 = 0;
int posEixo2 = 0;
int pos1Eixo3 = 0;
int posEixo3 = 0;
int pos1Eixo4 = 0;
int posEixo4 = 0;
int pos1Garra = 0;
int posGarra = 0;

Servo eixo1;
Servo eixo2;
Servo eixo3;
Servo eixo4;
Servo eixo5;
Servo garra;

Servos::Servos(uint8_t eixo1Pin, uint8_t eixo2Pin, uint8_t eixo3Pin, uint8_t eixo4Pin, uint8_t garraPin)
{
    _eixo1Pin = eixo1Pin;
    _eixo2Pin = eixo2Pin;
    _eixo3Pin = eixo3Pin;
    _eixo4Pin = eixo4Pin;
    _garraPin = garraPin;
}

void Servos::inicializacao()
{
    eixo1.setPeriodHertz(50); //MG995 -> 50Hz
    eixo2.setPeriodHertz(50);
    eixo3.setPeriodHertz(50);
    eixo4.setPeriodHertz(50);
    garra.setPeriodHertz(50);

    eixo1.attach(_eixo1Pin, 500, 2000); //MG995
    eixo2.attach(_eixo2Pin, 600, 2400); //MG90S
    eixo3.attach(_eixo3Pin, 500, 2500);
    eixo4.attach(_eixo4Pin, 500, 2500);
    garra.attach(_garraPin, 500, 2500);
}

void Servos::sendMoves(uint16_t leituraEixo1, uint16_t leituraEixo2, uint16_t leituraEixo3, uint16_t leituraEixo4, uint16_t leituraGarra)
{
    eixo1.write(leituraEixo1);
    eixo2.write(leituraEixo2);
    eixo3.write(leituraEixo3);
    eixo4.write(leituraEixo4);
    garra.write(leituraGarra);
    delay(servo_Delay);
}

void Servos::sendMoves(uint16_t leituraEixo1, uint16_t leituraEixo2, uint16_t leituraEixo3, uint16_t leituraEixo4, uint16_t leituraGarra, uint32_t servo_TempoEspera)
{
    eixo1.write(leituraEixo1);
    eixo2.write(leituraEixo2);
    eixo3.write(leituraEixo3);
    eixo4.write(leituraEixo4);
    garra.write(leituraGarra);
    delay(servo_TempoEspera);
}

void Servos::sendMovesSpeed(uint8_t eixo, uint16_t leituraEixo, uint8_t speed)
{
    int mapSpeed = map(speed, 0, 100, 100, 0); //0-100%
    switch (eixo)
    {
    case 1:
        if (leituraEixo > posEixo1)
        {
            for (posEixo1 = pos1Eixo1; posEixo1 <= leituraEixo; posEixo1 += 1)
            {
                eixo1.write(posEixo1);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Eixo1 = eixo1.read();
        }
        else if (leituraEixo < posEixo1)
        {
            for (posEixo1 = pos1Eixo1; posEixo1 >= leituraEixo; posEixo1 -= 1)
            {
                eixo1.write(posEixo1);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Eixo1 = eixo1.read();
        }
        break;
    case 2:
        if (leituraEixo > posEixo2)
        {
            for (posEixo2 = pos1Eixo2; posEixo2 <= leituraEixo; posEixo2 += 1)
            {
                eixo2.write(posEixo2);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Eixo2 = eixo2.read();
        }
        else if (leituraEixo < posEixo2)
        {
            for (posEixo2 = pos1Eixo2; posEixo2 >= leituraEixo; posEixo2 -= 1)
            {
                eixo2.write(posEixo2);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Eixo2 = eixo2.read();
        }
        break;
    case 3:
        if (leituraEixo > posEixo3)
        {
            for (posEixo3 = pos1Eixo3; posEixo3 <= leituraEixo; posEixo3 += 1)
            {
                eixo3.write(posEixo3);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Eixo3 = garra.read();
        }
        else if (leituraEixo < posEixo3)
        {
            for (posEixo3 = pos1Eixo3; posEixo3 >= leituraEixo; posEixo3 -= 1)
            {
                eixo3.write(posEixo3);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Eixo3 = eixo3.read();
        }
        break;
    case 4:
        if (leituraEixo > posEixo4)
        {
            for (posEixo4 = pos1Eixo4; posEixo4 <= leituraEixo; posEixo4 += 1)
            {
                eixo4.write(posEixo4);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Eixo4 = eixo4.read();
        }
        else if (leituraEixo < posEixo4)
        {
            for (posEixo4 = pos1Eixo4; posEixo4 >= leituraEixo; posEixo4 -= 1)
            {
                eixo4.write(posEixo4);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Eixo4 = eixo4.read();
        }
        break;
    case 5:
        if (leituraEixo > posGarra)
        {
            for (posGarra = pos1Garra; posGarra <= leituraEixo; posGarra += 1)
            {
                garra.write(posGarra);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Garra = garra.read();
        }
        else if (leituraEixo < posGarra)
        {
            for (posGarra = pos1Garra; posGarra >= leituraEixo; posGarra -= 1)
            {
                garra.write(posGarra);
                delay(mapSpeed);
                if (controlType1 != 2)
                {
                    break;
                }
            }
            pos1Garra = garra.read();
        }
        break;
    default:
        break;
    }
}

void Servos::checkControlType(uint8_t controlType)
{
    controlType1 = controlType;
}