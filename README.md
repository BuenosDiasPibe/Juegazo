# MonoGame game for my school networdking class im not joking

si el codigo tira errores en temas que no encuentra un archivo .csv, entrar al "TestScene.cs" y buscar en "LoadContent()" la variable CSVPath y cambiarlo por "../../../Data/sesFinal.csv"

si esto sigue sin funcionar, cambiar al estado original y intentar escribir en la terminal "dotnet run", ya que el juego cambia de buscar un ejecutable a correr directamente el juego, lo que cambia el puntero inicial en el que el juego busca por archivos