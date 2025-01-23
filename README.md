# [WIP] codeflix

###  Features:
- Assinatura do serviço 
- Catálogo de vídeos para navegação
- Playback de vídeos
- Busca full text no catálogo
- Processamento e encoding dos vídeos
- Administração do catálogo de vídeos
- Administração de serviço de assinatura
- Autenticação

### Decisões de Arquitetura:
- Arquitetura baseada em microsserviços
- Ajuste de tecnologia adequada para cada contexto do projeto (Ex. Go para processar vídeos, React.js para o frontEnd)
- Cada microsserviço terá seu próprio processo de CI/CD
- Escala:
  - O processo de escala poderá ser configurado a nível de microsserviço
  - Todos os microsserviçs trabalharão de forma "Stateless"
  - Quando utilizado upload de qualquer tipo de asset, o mesmo será armazenado em um Cloud Storage
  - O processo de escala se dará no aumento na quantidade de PODs do Kubernetes
  - O processo de autoscaling também será utilizado através de um recurso chamado HPA (Horizontal Pod Autoscaler)
