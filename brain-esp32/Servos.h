#ifndef Servos_h
#define Servos_h

#include "Arduino.h"
#include <ESP32Servo.h>

class Servos
{
public:
  Servos(uint8_t eixo1Pin, uint8_t eixo2Pin, uint8_t eixo3Pin, uint8_t eixo4Pin, uint8_t garraPin);
  void inicializacao();
  void sendMoves(uint16_t leituraEixo1, uint16_t leituraEixo2, uint16_t leituraEixo3, uint16_t leituraEixo4, uint16_t leituraGarra);
  void sendMoves(uint16_t leituraEixo1, uint16_t leituraEixo2, uint16_t leituraEixo3, uint16_t leituraEixo4, uint16_t leituraGarra, uint32_t servo_TempoEspera);
  void sendMovesSpeed(uint8_t eixo, uint16_t leituraEixo, uint8_t speed);
  void checkControlType(uint8_t controlType);

private:
  uint8_t _eixo1Pin;
  uint8_t _eixo2Pin;
  uint8_t _eixo3Pin;
  uint8_t _eixo4Pin;
  uint8_t _garraPin;
  uint8_t controlType1;
};

#endif
