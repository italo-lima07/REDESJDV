using System;
using System.Net.Sockets;
using System.Text;

class TicTacToeClient
{
    private TcpClient client; // Cliente TCP para comunicação com o servidor
    private NetworkStream stream; // Fluxo de rede para enviar e receber dados
    private const string server = "127.0.0.1"; // Endereço IP do servidor
    private const int port = 13000; // Porta do servidor
    private const int bufferSize = 512; // Tamanho do buffer para leitura de dados
    private byte[] buffer = new byte[bufferSize]; // Buffer para armazenar dados recebidos

    static void Main(string[] args)
    {
        var client = new TicTacToeClient(); // Cria uma instância do cliente
        client.Run(); // Inicia a execução do cliente
    }

    public void Run()
    {
        try
        {
            ConnectToServer(); // Conecta ao servidor
            PlayGame(); // Inicia o loop do jogo
        }
        catch (SocketException e)
        {
            // Exibe uma mensagem de erro caso ocorra uma exceção de socket
            Console.WriteLine($"SocketException: {e.Message}");
        }
        finally
        {
            CloseConnection(); // Fecha a conexão com o servidor
        }
    }

    private void ConnectToServer()
    {
        // Conecta ao servidor usando o endereço IP e a porta especificados
        client = new TcpClient(server, port);
        stream = client.GetStream(); // Obtém o fluxo de rede do cliente
        Console.WriteLine("Conectado ao servidor do Jogo da Velha!");
    }

    private void PlayGame()
    {
        string playerSymbol = "O"; // Símbolo do jogador, padrão é "O"
        bool gameStarted = false; // Indica se o jogo começou
        bool gameEnded = false; // Indica se o jogo terminou
        bool control = false; // Indica se o jogador tem controle para jogar

        // Espera até que o jogo comece
        while (!gameStarted)
        {
            string startMsg = ReadServerMessage().Trim(); // Lê a mensagem inicial do servidor

            // Verifica a mensagem recebida para determinar o estado do jogo
            if (startMsg == "1")
            {
                Console.WriteLine("Aguardando outro jogador...");
                playerSymbol = "X"; // Se "1", o jogador atual é "X"
            }
            else if (startMsg == "0")
            {
                Console.WriteLine("Iniciando Partida...");
                gameStarted = true; // Marca que o jogo começou
            }
        }

        // Recebe e exibe o tabuleiro inicial do jogo
        string response = ReadServerMessage();
        DisplayBoard(response);

        if (playerSymbol == "O")
        {
            control = true; // Se o símbolo do jogador for "O", ele tem controle inicial
        }

        // Loop principal do jogo
        while (!gameEnded)
        {
            if (control)
            {
                Console.WriteLine("Aguarde a jogada do seu oponente...");
            }

            // Lê a mensagem do servidor
            response = ReadServerMessage();

            // Se a resposta do servidor for um único caractere, é a vez do jogador
            if (response.Length == 1)
            {
                ProcessPlayerTurn(ref response, ref gameEnded, playerSymbol); // Processa a jogada do jogador
            }

            // Exibe o tabuleiro atualizado
            DisplayBoard(response);

            // Verifica o resultado do jogo
            gameEnded = CheckGameResult(response, playerSymbol);
            
            // Alterna o controle entre os jogadores
            control = !control;
        }
    }

    private string ReadServerMessage()
    {
        // Lê uma mensagem do servidor e a retorna como string
        int bytesRead = stream.Read(buffer, 0, bufferSize);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    private void ProcessPlayerTurn(ref string response, ref bool gameEnded, string playerSymbol)
    {
        while (true)
        {
            // Solicita a jogada do jogador
            Console.WriteLine("Sua vez! Digite sua jogada (1-9):");
            string jogada = Console.ReadLine();
            SendMessageToServer(jogada); // Envia a jogada para o servidor

            // Recebe a confirmação da jogada
            string serverResponse = ReadServerMessage().Trim();

            if (serverResponse == "-1")
            {
                Console.WriteLine("Jogada inválida, tente novamente."); // Jogada inválida
            }
            else
            {
                Console.WriteLine("Jogada registrada com sucesso!");
                response = ReadServerMessage(); // Recebe o tabuleiro atualizado do servidor
                break; // Sai do loop se a jogada for válida
            }
        }
    }

    private void SendMessageToServer(string message)
    {
        // Converte a mensagem para bytes e envia para o servidor
        byte[] msg = Encoding.UTF8.GetBytes(message);
        stream.Write(msg, 0, msg.Length);
    }

    private bool CheckGameResult(string response, string playerSymbol)
    {
        if (response.Length == 10)
        {
            char gameResult = response[9]; // Resultado do jogo está no último caractere da resposta
            if (gameResult == '1' || gameResult == '2')
            {
                DisplayResult(gameResult, playerSymbol); // Exibe o resultado do jogo
                return true; // O jogo terminou
            }
            else if (gameResult == '3')
            {
                Console.WriteLine("Jogo Encerrado, Deu Velha! (Empate)");
                return true; // O jogo terminou
            }
        }
        return false; // O jogo continua
    }

    private void DisplayResult(char gameResult, string playerSymbol)
    {
        if ((gameResult == '1' && playerSymbol == "X") || (gameResult == '2' && playerSymbol == "O"))
        {
            Console.WriteLine("Você venceu!"); // Mensagem de vitória
        }
        else
        {
            Console.WriteLine("Infelizmente, o oponente venceu."); // Mensagem de derrota
        }
    }

    private void DisplayBoard(string board)
    {
        // Exibe o estado atual do tabuleiro
        Console.WriteLine("");
        Console.WriteLine("Jogo da Velha Atual:");
        Console.WriteLine($"{board[0]} | {board[1]} | {board[2]}");
        Console.WriteLine("--+---+--");
        Console.WriteLine($"{board[3]} | {board[4]} | {board[5]}");
        Console.WriteLine("--+---+--");
        Console.WriteLine($"{board[6]} | {board[7]} | {board[8]}");
        Console.WriteLine("");
    }

    private void CloseConnection()
    {
        // Fecha o fluxo e o cliente
        stream?.Close();
        client?.Close();
        Console.WriteLine("Conexão encerrada.");
    }
}