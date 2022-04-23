namespace ExtraDry.Core.Tests.Rules;

public class RuleEngineCreatePaymentTests {

    [Fact]
    public async Task CheckPayment()
    {
        var rules = new RuleEngine(new ServiceProviderStub());
        var payment = new Payment();

        payment = await rules.CreateAsync(payment);

        Assert.NotNull(payment);
    }

}

/// <summary>
/// Represents a payment into a system to fulfil (or partially fulfil) an Order.
/// Typically a Stripe, ZipPay or PayPal transaction.  In the future could also be a voucher redemption.
/// </summary>
public class Payment {

    /// <summary>
    /// The unique key for the payment.
    /// </summary>
    [Key]
    [JsonIgnore]
    [Rules(RuleAction.Ignore)]
    public int Id { get; set; }

    ///// <summary>
    ///// The Order that this payment is for.
    ///// </summary>
    //[Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    //public Order Order { get; set; }

    ///// <summary>
    ///// Represents the payment state during processing.
    ///// </summary>
    //[Rules(RuleAction.Allow)]
    //public PaymentState State { get; set; }

    ///// <summary>
    ///// The list of payment events associated with this payment.
    ///// </summary>
    //public List<PaymentEvent> Events { get; init; } = new List<PaymentEvent>();

    ///// <summary>
    ///// The details on the processor and their processing information.
    ///// </summary>
    //[NotMapped]
    //public PaymentProcessor Processor { get; set; }

    /// <summary>
    /// The processor specific ID for this payment, type and format will vary amongst providers.
    /// </summary>
    [StringLength(100)]
    [Rules(RuleAction.Allow)]
    public string ProcessorId { get; set; }

    ///// <summary>
    ///// The document-mapped version of Processor.
    ///// </summary>
    //// TODO: Move to EF translation.
    //[JsonIgnore]
    //public string ProcessorJson {
    //    get => JsonSerializer.Serialize(Processor);
    //    set {
    //        Processor = JsonSerializer.Deserialize<PaymentProcessor>(value);
    //    }
    //}

    ///// <summary>
    ///// Place ProcessorName in database for fast summary queries.
    ///// </summary>
    //[JsonIgnore]
    //[MaxLength(50)]
    //[Rules(RuleAction.Allow)]
    //public string ProcessorName {
    //    get => Processor.Name;
    //    set { }
    //}

    ///// <summary>
    ///// The participant who is responsible for the billing of the order.
    ///// Treating the parents as participants in their own right.
    ///// </summary>
    //public Participant BillingParticipant { get; set; }

    /// <summary>
    /// The amount that is charged as part of this payment.
    /// </summary>
    [Range(0, 50000)]
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    [Precision(18, 2)]
    public decimal Amount { get; set; }

    /// <summary>
    /// The portion of the `Amount` that is displayed to the user as a merchant fee.
    /// This is inclusive of GST, use `AdvertisedMerchangtFeeExGst` and `AdvertisedMerchangtFeeGst` to get components.
    /// This is just a single percentage for display to customers which is averaged across all payments for 12 months to approximate the `ActualMerchantFee`.
    /// </summary>
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    [Precision(18, 2)]
    public decimal AdvertisedMerchantFee { get; set; }

    ///// <summary>
    ///// The portion of the `AdvertisedMerchantFee` that isn't GST.
    ///// </summary>
    //[NotMapped]
    //public decimal AdvertisedMerchantFeeExGst {
    //    get => SodMath.RoundSafe(AdvertisedMerchantFee, 1 + IncludedTaxRate);
    //}

    ///// <summary>
    ///// The amount of the `AdvertsiedMerchantFee` that is collected as GST.
    ///// </summary>
    //public decimal AdvertisedMerchantFeeGst {
    //    get => AdvertisedMerchantFee - AdvertisedMerchantFeeExGst;
    //}

    ///// <summary>
    ///// The rate that was charged with this payment.
    ///// </summary>
    //public decimal AdvertisedMerchantRateExGst => SodMath.RateFromFeeAndGst(Amount, AdvertisedMerchantFee, IncludedTaxRate);

    /// <summary>
    /// The tax rate that applies to this order item.
    /// While AU GST has been constant for a while, it could change over time.
    /// </summary>
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    [Precision(18, 2)]
    public decimal IncludedTaxRate { get; set; }

    /// <summary>
    /// The portion of the amount that is charged that is being used offset the processors fee.
    /// This is typically a percent plus a flat rate, e.g. Stripe is 1.75% + $0.30.
    /// </summary>
    [Rules(CreateAction = RuleAction.Allow, UpdateAction = RuleAction.Block)]
    [Precision(18, 2)]
    public decimal ActualMerchantFee { get; set; }

    /// <summary>
    /// An optional message from the payment processor, typically extracted from a rejected payload.
    /// </summary>
    [MaxLength(250)]
    [Rules(RuleAction.Allow)]
    public string Message { get; set; }

    /// <summary>
    /// The auto-populated version information for the provider.
    /// </summary>
    [Required]
    [JsonIgnore]
    public VersionInfo VersionInfo { get; set; } = new();
}
