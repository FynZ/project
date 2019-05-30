# Microservice project

This is a microservice architectured project acting as a platform to help players achieve the OCHRE DOFUS in the online multiplayer game [Dofus](https://www.dofus.com/en).

Although this application has a valid objective, it's mainly done for learning purpose.

# Structure

Each part of the application is a standalone project, containing:

## API Gateway

Spring boot application using Zuul and an Eureka Client to forward requests to the corresponding microservice.
Sources are under src/gateway

## Service Discovery

Spring boot application using Eureka Server.
Sources are under src/eureka

## Accounts Microservice

.NET Core application managing accounts, login and registering.
Sources are under src/Accounts

## Monster Microservice

.NET Core application managing user monsters.
Sources are under src/Monsters

## Trading Microservice

.NET Core application managing trading.
Sources are under src/Trading

## Message Microservice (TBD)

Application for managing user messages.

## News Microservice

Spring boot application managing news and comments.
Sources are under src/news

## Ressources Microservice

Spring boot application managing web ressources.
Sources are under src/ressources

## Front

Angular 7 application for the web view.
Sources are under src/front


# Deployment

Deployment is done using cake to setup a local project with all it's dependencies, including:

### PostgreSQL database
### ElasticSearch server
### Kibana instance
### RabbitMQ server
### Portainer 

## To Build

Clone the repository
Run commands

    ./build.ps1
    ./build.ps1 -t SetupDevEnvironment --env=dependencies
    ./build.ps1 -t SetupDevEnvironment
Go to `src/front`
Run commands

    npm install
    ng serve
Open app at localhost:4200