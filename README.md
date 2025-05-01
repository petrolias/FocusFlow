# FocusFlow

# Initial Setup

```
dotnet tool install --global dotnet-ef
```

Verify Installation
```
dotnet ef --version
```

## Migrations
Create the Migration
``

dotnet ef migrations add InitialCreate --context Context --project ./FocusFlow.Core --startup-project ./FocusFlow.WebApi
dotnet ef migrations add InitialCreate --context Context
``

Apply the Migration
``
dotnet ef database update --context FocusFlowContext
``