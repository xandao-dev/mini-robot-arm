/*-----------------------------------------------------------------------------------------------
Essa biblioteca foi criada para enviar as leituras dos potenciometros para quem quiser chama-la.
-------------------------------------------------------------------------------------------------*/

#include "Potenciometro.h"

Potenciometro::Potenciometro(uint8_t potEixo1, uint8_t potEixo2, uint8_t potEixo3, uint8_t potEixo4, uint8_t potGarra)
{
  _potEixo1 = potEixo1;
  _potEixo2 = potEixo2;
  _potEixo3 = potEixo3;
  _potEixo4 = potEixo4;
  _potGarra = potGarra;
}

void Potenciometro::potRead(uint16_t *leiturasPotenciometro)
{
  potEixo1Valor = analogRead(_potEixo1); //0 ate 4095
  potEixo2Valor = analogRead(_potEixo2);
  potEixo3Valor = analogRead(_potEixo3);
  potEixo4Valor = analogRead(_potEixo4);
  potGarraValor = analogRead(_potGarra);

  potEixo1Valor = map(potEixo1Valor, 0, 4095, 0, 180); //mudando pra 1-180
  potEixo2Valor = map(potEixo2Valor, 0, 4095, 0, 180);
  potEixo3Valor = map(potEixo3Valor, 0, 4095, 0, 180);
  potEixo4Valor = map(potEixo4Valor, 0, 4095, 0, 180);
  potGarraValor = map(potGarraValor, 0, 4095, 0, 180);

  uint16_t read[5] = {potEixo1Valor, potEixo2Valor, potEixo3Valor, potEixo4Valor, potGarraValor};
  for (int i = 0; i < 5; i++)
  {
    leiturasPotenciometro[i] = read[i]; //leituras[i] === *(leituras + i)
  }
}
