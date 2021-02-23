# Project Title

Todo list app

### Getting Started

Entry point is src/Service project, can be run in debug mode to access todolsit functionality locally 

### About

Todo list app for code interview test. Based on react-redux + asp.net core web api template.

Solution is splitted into several layers to better match n-tier approach.

Layer | Project/folder @  | Description
--- | --- | ---
Cross-cutting | Shared | Exception model
DataAccess | DataAccess.Abstraction | Entities and interface for the repository
DataAccess | DataAccess.Inmemory | Inmemory implementation of the repository
BussinessLogic | BussinessLogic.Abstraction | Model and interfaces for action handlers
BussinessLogic | BussinessLogic.Handlers | Implementation of bussiness logic abstractions. Validators are contained here as an implementation detail
Service | Service.Abstraction | Model used by asp.net  service controller
Service | Service | asp.net core web-api
Presentation | Presentation | react spa app