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
- Consistência eventual:
  - Grande parte da comunicação entre os microsservições será assíncrona
  - Cada microsserviço possuirá sua própria base de dados
  - Eventualmente os dados poderão ficar inconsistentes, desd que não haja prejuízo direto ao negócio
- Duplicação de dados:
  - Eventualmente um microsserviço poderá persistir dados já existentes em outro microsserviço em seu banco de dados
  - Essa duplicação ocorre para deixar o microsserviço mais autônomo e preciso
  - O microsserviço duplicará apenas os dados necessários para seu contexto
  - No caso da Codeflix utilizaremos o Kafka Connect como replicador de dados
- Mensageria:
  - Como parte da comunicação entre os microsserviços é assíncrona, um sistema de mensageria é necessário
  - O RabbitMQ foi escolhido para esse caso
  - Por que não o Apache Kafka ou Amazon SQS, entre outross?
    -  Apache Kafka também poderia ser utilizado nesse caso, por outro lado, decidimos utilizar juntamente com o Kafka Connect apenas para replicação de dados, visto que o Kafka vai muito além das mensagerias
    -  Evitaremos o Lock-In nos cloud providers
