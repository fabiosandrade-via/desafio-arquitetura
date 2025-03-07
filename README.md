# 🚀 Projeto: Sistema de Lançamentos e Consolidação Diária

Este projeto tem como objetivo fornecer uma plataforma para lançamento e consolidação de dados financeiros. Ele foi desenvolvido utilizando uma arquitetura baseada em microsserviços, garantindo escalabilidade e manutenibilidade. A aplicação é composta por um frontend **Angular 19** e dois microsserviços backend desenvolvidos em **.NET Core 8**, todos integrados por meio do Docker

![Arquitetura do Sistema](./doc/arquitetura/arquitetura.png)

### **🔴 Pontos de atenção**

Gostaria de ter implementado, porém devido a falta de infraestrutura, budget e tempo para entrega, não foram implementadas conforme o desenho arquitetural os componentes referentes as tecnologias listadas abaixo:
 - Akamai;
 - Subida em cloud;
 - Pipeline CI/CD;
 - Microserviço Login;
 - Autenticação/Autorização;
 - Loadbalancer;
 - API Gateway;
 - Kubernates;
 - VPN.
---

## 📌 Tecnologias Utilizadas

- **Frontend:** Angular 19, TypeScript, RxJS, Angular Forms
- **Backend:** .NET Core 8 (dois microserviços)
- **Banco de Dados:** MongoDB
- **Cache:** Redis
- **Mensageria:** Kafka
- **Monitoramento:** Prometheus e Grafana
- **Containerização:** Docker e Docker Compose

---

## 📝 Justificativa das stacks utilizadas

- **Angular 19, TypeScript, RxJS, Angular Forms:** Permite a reutilização de componentes, melhorando a organização e manutenção do código.
- **.NET Core 8 (dois microserviços):** Estruturado em dois microserviços independentes, arquitetura baseada em microserviços permitindo escalar individualmente. Funciona em Windows, Linux e MacOS cada serviço na qual o código é modular, fácil manutenção e integração com múltiplos serviços.
- **MongoDB:** Suporte a dados dinâmicos, sem necessidade de esquemas rígidos além de gilidade no armazenamento e recuperação de dados sem comprometer o desempenho.
- **Redis:** Pensando na alta disponibilidade de relatório do fluxo de caixa tem baixa latência, escalabilidade resultando na diminuição da carga sobre o banco de dados, melhorando a performance da aplicação
- **Kafka:** Foi escolhido na utilização no contexto de indisponibilidade do serviço de consolidado-diario. Caso ocorra, será realizada comunicação assíncrona entre os microserviços. A execução do serviço consumidor é feita de forma independente da API rodando em background (HostedService).
- **Prometheus e Grafana:** Considero como ferramentas player de mercado para monitorarento de métricas e logs da aplicação além do bom custo benefício.
- **Docker e Docker Compose:**  A aplicação foi containerizada para garantir consistência no ambiente de execução a partir do isolamento do ambiente, ou seja, o código roda da mesma forma em qualquer sistema operacional além da facilidade do deploy e da escalabilidade.

---

## 📁 Estrutura do Projeto

```
├── frontend-angular (Angular 19)
│   ├── src
│   │   ├── app
│   │   │   ├── lancamento
│   │   │   ├── consolidado-diario
│   │   │   ├── services
│   │   │   ├── components
│   │   │   ├── styles.css
│   │   ├── index.html
│   │   ├── main.ts
│   │   ├── ...
│   ├── angular.json
│   ├── package.json
│   ├── Dockerfile
│
├── backend-lancamento (.NET Core 8)
│   ├── MinimalAPI
│   ├── Serviço
│   ├── Repositório
│   ├── Domínio
│   ├── Integração
│   ├── Mensageria
│   ├── appsettings.json
│   ├── Program.cs
│   ├── Dockerfile
│
├── backend-consolidado (.NET Core 8)
│   ├── MinimalAPI
│   ├── Serviço
│   ├── Repositório
│   ├── Domínio
│   ├── Mensageria
│   ├── appsettings.json
│   ├── Program.cs
│   ├── Dockerfile
│
├── docker-compose.yml
├── README.md
```

---

## 🛠️ Configuração e Execução do Projeto

### **1️⃣ Clonar o Repositório**

```sh
git clone https://github.com/fabiosandrade-via/desafio-arquitetura
cd desafio-arquitetura
```

### **2️⃣ Construir e Subir os Containers**

```sh
docker-compose up --build
```

### **3️⃣ Acessar a Aplicação**

- **Frontend Angular:** [http://localhost:4200](http://localhost:4200)
- **Backend Lançamentos:** [http://localhost:8080/api/lancamentos](http://localhost:8080/api/lancamentos)
- **Backend Consolidação:** [http://localhost:8081/api/lancamentos/consolidado](http://localhost:8081/api/lancamentos/consolidado)
- **Grafana:** [http://localhost:3000](http://localhost:3000)
- **Prometheus:** [http://localhost:9090](http://localhost:9090)

---
### **🎨 Evidências de resultados do projeto**
#### Página de lançamentos para o fluxo de caixa
![Tela de Lancamentos](./doc/img/lancamentos.png)
#### Página de consolidado diário
![Tela de Consolidado Diário](./doc/img/consolidado-diario.png)
#### Grafana
![Tela Grafana](./doc/img/grafana_consolidado.png)
#### API Lançamentos
![API Lançamentos](./doc/img/api-lancamentos.png)
#### API Consolidado Diário
![API Consolidado](./doc/img/api-consolidado.png)
#### MongoDB
![MongoDB](./doc/img/mongodb.png)
#### Redis
![Redis](./doc/img/redis.png)
#### Resiliênca - Retry/CircuitBreaker, usando biblioteca Polly
![Resiliência](./doc/img/retry-circuitbreaker.png)
#### Kafka
![Kafka](./doc/img/kafka.png)
---

### **📌 Considerações Finais**
Este projeto foi desenvolvido visando escalabilidade, separação de responsabilidades e facilidade de manutenção, conforme solicitado no documento **desafio-arquiteto-solucao-ago2024.pdf**. Com a arquitetura baseada em microsserviços e containerização, o sistema pode ser facilmente expandido conforme as necessidades do negócio.