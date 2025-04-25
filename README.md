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

## Validação de DTOs vs. Modelos de Domínio

Neste projeto, implementamos validadores tanto para o DTO (CustomerDto) quanto para o modelo de domínio (Customer), mas é importante observar que:

1. **Na maioria dos casos, validar apenas o DTO é suficiente**, pois:
   - Os DTOs são as classes que recebem dados diretamente das requisições HTTP
   - A validação deve ocorrer no ponto de entrada dos dados, antes de qualquer processamento
   - O modelo de domínio é geralmente populado a partir de um DTO já validado

2. **A validação do modelo de domínio geralmente é redundante quando:**
   - Ele é populado a partir de um DTO já validado
   - Não contém regras de negócio adicionais que não se aplicam ao DTO

3. **A validação do modelo de domínio pode ser necessária quando:**
   - Ele é manipulado diretamente sem passar por um DTO
   - Possui regras específicas de domínio que não cabem no DTO
   - É utilizado em processos internos onde não há um DTO como intermediário

A abordagem mais comum e eficiente é validar apenas no ponto de entrada (DTO) e garantir que a conversão para o modelo de domínio preserva a integridade dos dados.

Para mais detalhes, consulte a [documentação oficial do FluentValidation](https://fluentvalidation.net/).