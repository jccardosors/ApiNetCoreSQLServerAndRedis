namespace Project.Application.ViewModels
{
    /// <summary>
    /// Classe que representa o lançamento
    /// </summary>
    public class EntryVM
    {
        /// <summary>
        /// Identificador do lançamento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descrição do lançamento
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal ValueEntry { get; set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime DateEntry { get; set; }

        /// <summary>
        /// Tipo do lançamento (True = Crédito, False = Débito)
        /// </summary>
        public bool IsCredit { get; set; }
    }
}
