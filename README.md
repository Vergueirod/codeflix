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
- Resiliência e Self Healing:
  - Para garantir resiliência caso um ou mais microsserviços fiquem fora do ar, as filas serão essenciais
  - Caso uma menagem venha em um padrão não esperado para determinado microsserviço, o microsserviço poderá rejeitá-la e automaticamente a mesma poderá ser encaminhada para uma dead-letter queue
  - Pelo fato do Kubernetes e Istio possuirem recursos de Circuit Breaker e Liveness e Readiness probes:
    - Se um container tiver um crash, automaticamente ele será reiniciado ou mesmo recriado
    - Caso um container não aguente determinado tráfego, temos a opção de trabalhar com Circuit Breaker para impedir que ele receba mais requisições enquanto está se "curando"
- Autenticação:
  -  Serviço centralizado de identidade opensource: Keycloak
  -  OpenID Connect
  -  Customização do tema
    - Utilização do create-react-app
  - Compartilhamento de chabe pública com os serviços para verificação de autenticidade dos tokens
  - Diversos tipos de ACL
  - Flow de autenticação para frontend e backend
 
### Microsserviços:
- Backend Admin do Catálogo de Vídeos
- Frontend Admin do Catálogo de Vídeos
- Encoder e Vídeos
- Backend API do Catálogo
- Frontend do Catálogo
- Assinatura do Codeflix pelo cliente
- Autenticação entre Microsserviços com Keycloak
- Comunicação assíncrona entre os Microsserviços com RabbitMQ
- Replicação de dados utilizando Apache Kafka e Kafka Connect

### Desenvolvimento e Deploy:
- Docker é o protagonista do ambiente de desenvolvimento
  - Permite a rápida criação do ambiente
  - Garante que os ambientes serão exatamente os mesmos
  - Facilita a criação de recursos periféricos como banco de dados, RabbitMQ e etc..
  - Geração de imagens para o ambiente de produção
-  CI / CD:
  - Para cada pull request gerada em uma aplicação, iniciaremos o processo de CI
  - Github Actions
  - O processo de CI será capaz de:
    - Subir a aplicação usando Docker
    - Executar os testes
    - Utilizar o Sonarqube
-  No caso de acontecer o "merge" da Pull Request, o processo de CD acontece
-  Fará a geração da imagem Docker
-  Realizará o upload da imagem em um container registry
-  Executará o deploy no Kubernetes
