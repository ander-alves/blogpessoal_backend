using blogPessoal.Model;
using blogPessoal.Service;
using FluentValidation;

namespace blogPessoal.Validator
{
    public class TemaValidator : AbstractValidator<Tema>
    {
        private readonly ITemaService _temaService;

        public TemaValidator(ITemaService temaService)
        {
            _temaService = temaService;

            RuleFor(d => d.Descricao)
                .NotEmpty().WithMessage("A descrição não pode estar vazia.")
                .MaximumLength(100).WithMessage("A descrição não pode ter mais de 100 caracteres.")
                .Matches("^[A-Za-zÀ-ú0-9.,\\-\\s]+$")
                .WithMessage("A descrição pode conter apenas letras, números, espaços, vírgulas e traços.")
                .MustAsync(NotBeDuplicateDescription).WithMessage("A descrição já está em uso.");
        }

        private async Task<bool> NotBeDuplicateDescription(string descricao, CancellationToken cancellationToken)
        {
            // Verifica se a descrição já está em uso
            var temas = await _temaService.GetByDescricao(descricao);

            // A descrição não é duplicada se nenhum tema foi encontrado ou se todos os temas têm ID 0 (indicando novas entradas)
            return temas == null || !temas.Any() || temas.All(t => t.Id == 0);
        }

    }
}
