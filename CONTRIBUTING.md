# Contributing

## Purpose
This repository contains the User Management API used for the TechHive Solutions assignment. Follow these guidelines to contribute.

## Coding Standards
- Follow .editorconfig rules (4-space indentation, CRLF line endings).
- PascalCase for types and public methods, camelCase for parameters.
- Private fields should start with an underscore (e.g., _cache).

## Pull Requests
- Create a branch named feature/xxxx or fix/xxxx.
- Provide a short description of changes.
- Ensure the API builds and all endpoints function as expected.

## Testing
- Use Postman or similar to test CRUD endpoints.
- Provide example requests and environment variables in PR description.

## Copilot
- Indicate where Copilot suggestions were used and the prompt used for larger changes.

## Middleware order
- Exception handling middleware first.
- Authentication middleware second.
- Logging middleware last.