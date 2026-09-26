The PayOS library provides convenient access to the payOS API from applications written in server .Net Core.

## Documentation

See the [payOS API docs](https://payos.vn/docs/api/) for more infomation.

## Installation

Install the package with:

```bash
    dotnet add package payOS
```

## Usage

### Initialize

You need to initialize the PayOS object with the Client ID, Api Key and Checksum Key of the payment channel you created, your Partner Code is optional.

- Common C#

```c#
using Net.payOS;

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"),
                    configuration["Environment:PAYOS_PARTNER_CODE"] ?? throw new Exception("Cannot find environment"));

```

### Methods included in the PayOS object

- **createPaymentLink**

Create a payment link for the order data

Syntax:

```c#
payOS.createPaymentLink(paymentData);
```

Parameter data type:

```c#
namespace Net.payOS.Types;

//PaymentData Type
public record PaymentData(
    long orderCode,
    int amount,
    String description,
    List<ItemData> items,
    String cancelUrl,
    String returnUrl,
    String ?signature = null,
    String ?buyerName = null,
    String ?buyerEmail = null,
    String ?buyerPhone = null,
    String ?buyerAddress = null,
    int ?expiredAt = null
);

//ItemData Type

public record ItemData(
    String name,
    int quantity,
    int price
);

```

Return data type:

```c#
namespace Net.payOS.Types;

public record CreatePaymentResult(
    String bin,
    String accountNumber,
    int amount,
    String description,
    long orderCode,
    String paymentLinkId,
    String status,
    String checkoutUrl,
    String qrCode
);
```

Example:

```c#
using Net.payOS.Types;

long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();
ItemData item = new ItemData("Mì tôm hảo hảo ly", 1, 1000);
List<ItemData> items = new List<ItemData>();
items.Add(item);
PaymentData paymentData = new PaymentData(orderCode, 1000, "Thanh toan don hang", items, "https://localhost:3002/cancel", "https://localhost:3002/success");

CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
```

- **getPaymentLinkInformation**

Get payment information of an order that has created a payment link.

Syntax:

```c#
payOS.getPaymentLinkInformation(id);
```

Parameters:

- `id`: Store order code (`orderCode`) or payOS payment link id (`paymentLinkId`). Type of `id` is long.

Return data type:

```c#
namespace Net.payOS.Types;

public record PaymentLinkInformation(
    String id,
    long orderCode,
    int amount,
    int amountPaid,
    int amountRemaining,
    String status,
    String createdAt,
    List<Transaction> transactions,
    String? canceledAt,
    String? cancellationReason
);

```

Transaction type:

```c#
namespace Net.payOS.Types;

public record Transaction(
    String reference,
    int amount,
    String accountNumber,
    String description,
    String transactionDateTime,
    String? virtualAccountName,
    String? virtualAccountNumber,
    String? counterAccountBankId,
    String? counterAccountBankName,
    String? counterAccountName,
    String? counterAccountNumber
);
```

Example:

```c#
using Net.payOS.Types;
PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(1);
```

- **cancelPaymentLink**

Cancel the payment link of the order.

Syntax:

```c#
payOS.cancelPaymentLink(orderCode, cancellationReason);
```

Parameters:

- `id`: Store order code (`orderCode`) or payOS payment link id (`paymentLinkId`). Type of `id` is long.

- `cancellationReason`: Reason for canceling payment link (optional).

Return data type:

```c#
namespace Net.payOS.Types;

public record PaymentLinkInformation(
    String id,
    long orderCode,
    int amount,
    int amountPaid,
    int amountRemaining,
    String status,
    String createdAt,
    List<Transaction> transactions,
    String? canceledAt,
    String? cancellationReason
);

```

Example:

```c#
using Net.payOS.Types;

long orderCode = 123;
String cancellationReason = "reason";

PaymentLinkInformation cancelledPaymentLinkInfo = payOS.cancelPaymentLink(orderCode, cancellationReason);

// If you want to cancel the payment link without reason:
PaymentLinkInformation cancelledPaymentLinkInfo = payOS.cancelPaymentLink(orderCode);
```

- **confirmWebhook**

Validate the Webhook URL of a payment channel and add or update the Webhook URL for that Payment Channel if successful.

Syntax:

```c#
payOS.confirmWebhook("https://your-webhook-url/")
```

- **verifyPaymentWebhookData**

Verify data received via webhook after payment.

Syntax:

```c#

payOS.verifyPaymentWebhookData(body);

```

Return data type:

```c#
namespace Net.payOS.Types;

public record WebhookData(
    long orderCode,
    int amount,
    String description,
    String accountNumber,
    String reference,
    String transactionDateTime,
    String paymentLinkId,
    String code,
    String desc,
    String? counterAccountBankId,
    String? counterAccountBankName,
    String? counterAccountName,
    String? counterAccountNumber,
    String? virtualAccountName,
    String virtualAccountNumber
);

public record WebhookType(
    String code,
    String desc,
    Boolean success,
    WebhookDataType webhookDataType,
    String signature
);
```

Example:

```c#
using Net.payOS.Types;

WebhookType body = ...; //receive from PayOS

WebhookData paymentData = payOS.verifyPaymentWebhookData(body);

```
