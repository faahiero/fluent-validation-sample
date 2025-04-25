# Exemplo de FluentValidation em ASP.NET Core

Este é um projeto simples demonstrando o uso do FluentValidation em uma aplicação ASP.NET Core Web API.

## Estrutura do Projeto

```
FluentValidationApi/
├── Controllers/
│   └── CustomersController.cs       # Controlador de clientes
├── Models/
│   ├── Customer.cs                  # Modelo de domínio
│   └── CustomerDto.cs               # DTO para transferência de dados
├── Validators/
│   ├── CustomerDtoValidator.cs      # Validador para o DTO
│   └── CustomerValidator.cs         # Validador para o modelo
├── Program.cs                       # Configuração da aplicação
└── appsettings.json                 # Configurações da aplicação
```

## Tecnologias Utilizadas

- ASP.NET Core 9.0
- FluentValidation.AspNetCore 11.3.0
- Swagger/OpenAPI

## Como Executar

1. Clone o repositório
2. Navegue até a pasta do projeto
3. Execute `dotnet run`
4. Acesse https://localhost:7113/swagger para testar a API

## Uso do FluentValidation

O FluentValidation é uma biblioteca para validação de objetos em .NET. Ela permite definir regras de validação de forma fluente e expressiva.

### Configuração

No arquivo `Program.cs`, configuramos o FluentValidation:

```csharp
// Configurando o FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CustomerDtoValidator>();
```

### Validadores

Os validadores são definidos como classes que herdam de `AbstractValidator<T>`:

```csharp
public class CustomerDtoValidator : AbstractValidator<CustomerDto>
{
    public CustomerDtoValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .Length(2, 50).WithMessage("O nome deve ter entre 2 e 50 caracteres");

        // Outras regras...
    }
}
```

### Uso nos Controladores

A validação é aplicada automaticamente nos controladores quando o modelo é vinculado:

```csharp
[HttpPost]
public ActionResult<Customer> Create(CustomerDto customerDto)
{
    // O modelo já foi validado automaticamente por causa do [ApiController]
    // Mas também podemos validar manualmente:
    var validationResult = _validator.Validate(customerDto);
    
    if (!validationResult.IsValid)
    {
        return BadRequest(validationResult.Errors);
    }

    // Processar o DTO válido...
}
```

## Exemplos de Regras de Validação

- **NotEmpty**: Verifica se um campo não está vazio
- **Length**: Valida o comprimento de uma string
- **EmailAddress**: Valida se é um email válido
- **Matches**: Usa expressões regulares para validação
- **LessThan**: Compara datas ou valores numéricos
- **Must**: Validação personalizada com função custom

Para mais detalhes, consulte a [documentação oficial do FluentValidation](https://fluentvalidation.net/). 