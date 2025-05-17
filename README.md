[x] Design DB (DBML): https://dbdiagram.io/d/EcommerceAPI-67c68657263d6cf9a01e40a8
 
[x] Create Project Structure

[x] Create Class Entity

[] Create BaseEntity + Audit Field

[x] Migration

[x] Repository

[] CQRS

[x] Service

[x] Controller

[x] JWT Auth

[x] Seed Data

[] Global Exception Middleware

[] Validation Fluent

[] Response Generic

[x] Unit Test + Integration Test

[] Deploy Azure or AWS

# Install

Follow these steps to set up the local project.

1. Start Docker Compose:  
   `docker compose up -d`

2. Start RabbitMQ:  
   `docker run -d --name hycommerce_rabbitmq -p 5672:5672 -p 15672:15672 -v hycommerce_rabbitmq_data:/var/lib/rabbitmq rabbitmq:management`