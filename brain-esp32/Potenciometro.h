#ifndef Potenciometro_h
#define Potenciometro_h

#include "Arduino.h"

class Potenciometro
{
public:
  Potenciometro(uint8_t potEixo1, uint8_t potEixo2, uint8_t potEixo3, uint8_t potEixo4, uint8_t potGarra);
  void potRead(uint16_t *leiturasPotenciometro);

private:
  uint8_t _potEixo1;
  uint8_t _potEixo2;
  uint8_t _potEixo3;
  uint8_t _potEixo4;
  uint8_t _potGarra;
  uint16_t potEixo1Valor;
  uint16_t potEixo2Valor;
  uint16_t potEixo3Valor;
  uint16_t potEixo4Valor;
  uint16_t potGarraValor;
};

#endif
