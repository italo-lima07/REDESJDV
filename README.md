```mermaid
graph TD
A(Iniciar Cliente) --> B[Conectar ao Servidor]
B --> C{Conexão Estabelecida?}
C -- Não --> D[Exibir Erro de Conexão]
C -- Sim --> E[Exibir Mensagem "Conectado ao Servidor"]
E --> F[Receber Mensagem Inicial do Servidor]
F --> G{Outro Jogador Conectado?}
G -- Não --> H[Exibir "Aguardando outro jogador..."]
G -- Sim --> I[Exibir "Iniciando Partida"]
I --> J[Receber e Exibir Tabuleiro Inicial]
J --> K{Jogador X?}
K -- Não --> L[Esperar Jogada do Oponente]
K -- Sim --> M[Solicitar Jogada ao Jogador]
L --> N[Receber e Exibir Tabuleiro Atualizado]
M --> O[Receber Jogada do Jogador]
O --> P[Enviar Jogada ao Servidor]
P --> Q{Jogada Válida?}
Q -- Não --> R[Exibir "Jogada Inválida" e Solicitar Nova Jogada]
Q -- Sim --> S[Receber Tabuleiro Atualizado]
S --> T[Exibir Tabuleiro Atualizado]
T --> U{Verificar Resultado do Jogo}
U -- Vitória --> V[Exibir "Você Venceu!"]
U -- Derrota --> W[Exibir "Você Perdeu"]
U -- Empate --> X[Exibir "Jogo Empatado!"]
V --> Z(Encerrar Conexão)
W --> Z
X --> Z
```
