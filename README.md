```mermad
graph TD
A(Iniciar Cliente) --> B[Conectar ao Servidor]
B --> C[Receber Mensagem do Servidor]
C --> D{Mensagem solicita jogada?}
D -- Sim --> E[Ler Jogada do Usuário]
E --> F[Enviar Jogada ao Servidor]
F --> G[Receber Atualização do Tabuleiro]
G --> H{Fim de Jogo?}
H -- Não --> C
H -- Sim --> I[Exibir Resultado Final]
I --> J[Encerrar Conexão]
D -- Não --> G
```
