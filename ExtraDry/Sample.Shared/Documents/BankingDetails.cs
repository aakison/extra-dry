namespace Sample.Shared {

    public class BankingDetails {

        [Display(Name = "Bank Number", Description = "The banks number such as an ABN in Australia or the R/T in the U.S.", ShortName = "ABN")]
        [MaxLength(20)]
        public string BankNumber { get; set; } = string.Empty;

        [Display(Name = "Account Name", Description = "The name as it appears on the account at the bank.")]
        [MaxLength(40)]
        public string AccountName { get; set; } = string.Empty;

        [Display(Name = "Account Number", Description = "The number of the bank account.")]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = string.Empty;

    }
}
