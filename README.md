# Project Title

Todo list app

### About

Todo list app for code interview test. Based on react-redux + asp.net core web api template.

### Getting Started

Check out the app on azure https://bx-todo-list.azurewebsites.net/
Download sources from github https://github.com/sokoliao/todo-list

### Requirements

Implementation | Description
--- | --- 
SPA React + Redux | Sources at the src/Server/ClientApp
ASP.NET as service side api | Sources at src/Server, asp.net web api on .NET core 5
Just in memory collection as database | src/Server/Stores/TodoTaskStore.cs provides basic get/set functionality to access inmemory collection

Actions | Description
--- | ---
List existing tasks | GetAll action of TodoTaskController dispatch query to retrieve items from store
Add and Edit tasks | Served by 2 different endpoints of TodoTaskController
Deletion of completed tasks only | Served by Delete endpoint of TodoTaskController, validation handled by src/Server/Validators/DeleteTaskValidator

Validation | Description
--- | ---
Every task has name, priority and status | Enforced by models on ui, controller's dto, internal model and "persistent" entity
Every task must have a name | CreateTodoTaskModel and UpdateTodoTaskModel dto have Required attribute, TodoEntityValidator enforce not-null constraint during create and update
Name is unique | NewTaskValidator and UpdateTaskValidator checking this constraint during create and update
Priority is number | Enforced by type of dto and TodoEntityValidator
Business may ask more validation in future | Additional rules may be added to fluent validators at backend and validation on front end

Unit and integration tests are available at tests/ServerTests