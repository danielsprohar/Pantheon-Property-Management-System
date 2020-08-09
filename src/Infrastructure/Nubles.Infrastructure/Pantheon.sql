IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [CreatedBy] int NOT NULL,
    [CreatedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [ModifiedBy] int NOT NULL,
    [ModifiedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [FirstName] nvarchar(32) NOT NULL,
    [MiddleName] nvarchar(32) NULL,
    [LastName] nvarchar(32) NOT NULL,
    [Gender] nvarchar(1) NOT NULL,
    [PhoneNumber] nvarchar(16) NOT NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(max) NULL,
    [IsActive] bit NULL,
    [DriverLicense_Number] nvarchar(32) NULL,
    [DriverLicense_State] nvarchar(32) NULL,
    [DriverLicense_PhotoUrl] nvarchar(max) NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [InvoiceStatuses] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [Description] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_InvoiceStatuses] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [ParkingSpaceTypes] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [SpaceType] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_ParkingSpaceTypes] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [PaymentMethods] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [Method] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [RentalAgreementTypes] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [AgreementType] nvarchar(16) NOT NULL,
    CONSTRAINT [PK_RentalAgreementTypes] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CustomerVehicle] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] varbinary(max) NULL,
    [Year] int NOT NULL,
    [Make] nvarchar(32) NOT NULL,
    [Model] nvarchar(64) NOT NULL,
    [Color] nvarchar(32) NOT NULL,
    [LicensePlateState] nvarchar(max) NULL,
    [LicensePlateNumber] nvarchar(max) NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_CustomerVehicle] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CustomerVehicle_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [ParkingSpaces] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [CreatedBy] int NOT NULL,
    [CreatedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [ModifiedBy] int NOT NULL,
    [ModifiedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [Name] nvarchar(4) NOT NULL,
    [Description] nvarchar(32) NULL,
    [RecurringRate] DECIMAL(10, 2) NOT NULL,
    [IsAvailable] bit NOT NULL DEFAULT CAST(1 AS bit),
    [Amps] int NULL,
    [Comments] nvarchar(2048) NULL,
    [ParkingSpaceTypeId] int NOT NULL,
    CONSTRAINT [PK_ParkingSpaces] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ParkingSpaces_ParkingSpaceTypes_ParkingSpaceTypeId] FOREIGN KEY ([ParkingSpaceTypeId]) REFERENCES [ParkingSpaceTypes] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Payments] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [CreatedBy] int NOT NULL,
    [CreatedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [ModifiedBy] int NOT NULL,
    [ModifiedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [Amount] DECIMAL(10, 2) NOT NULL,
    [Date] datetimeoffset NOT NULL,
    [IsRefund] bit NOT NULL DEFAULT CAST(0 AS bit),
    [PaymentMethodId] int NOT NULL,
    [CustomerId] int NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payments_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Payments_PaymentMethods_PaymentMethodId] FOREIGN KEY ([PaymentMethodId]) REFERENCES [PaymentMethods] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [RentalAgreements] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [CreatedBy] int NOT NULL,
    [CreatedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [ModifiedBy] int NOT NULL,
    [ModifiedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [RecurringDueDate] int NOT NULL,
    [TerminatedOn] datetimeoffset NULL,
    [RentalAgreementTypeId] int NOT NULL,
    [ParkingSpaceId] int NOT NULL,
    CONSTRAINT [PK_RentalAgreements] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RentalAgreements_ParkingSpaces_ParkingSpaceId] FOREIGN KEY ([ParkingSpaceId]) REFERENCES [ParkingSpaces] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RentalAgreements_RentalAgreementTypes_RentalAgreementTypeId] FOREIGN KEY ([RentalAgreementTypeId]) REFERENCES [RentalAgreementTypes] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [CustomerRentalAgreements] (
    [CustomerId] int NOT NULL,
    [RentalAgreementId] int NOT NULL,
    CONSTRAINT [PK_CustomerRentalAgreements] PRIMARY KEY ([CustomerId], [RentalAgreementId]),
    CONSTRAINT [FK_CustomerRentalAgreements_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CustomerRentalAgreements_RentalAgreements_RentalAgreementId] FOREIGN KEY ([RentalAgreementId]) REFERENCES [RentalAgreements] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Invoices] (
    [Id] int NOT NULL IDENTITY,
    [RowVersion] rowversion NULL,
    [CreatedBy] int NOT NULL,
    [CreatedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [ModifiedBy] int NOT NULL,
    [ModifiedOn] datetimeoffset NOT NULL DEFAULT (GETUTCDATE()),
    [DueDate] datetimeoffset NOT NULL,
    [BillingPeriodStart] datetimeoffset NOT NULL,
    [BillingPeriodEnd] datetimeoffset NOT NULL,
    [Comments] nvarchar(2048) NULL,
    [RentalAgreementId] int NOT NULL,
    [InvoiceStatusId] int NOT NULL,
    CONSTRAINT [PK_Invoices] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Invoices_InvoiceStatuses_InvoiceStatusId] FOREIGN KEY ([InvoiceStatusId]) REFERENCES [InvoiceStatuses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Invoices_RentalAgreements_RentalAgreementId] FOREIGN KEY ([RentalAgreementId]) REFERENCES [RentalAgreements] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [InvoiceLine] (
    [InvoiceId] int NOT NULL,
    [ParkingSpaceId] int NOT NULL,
    [Quantity] int NOT NULL,
    [Description] nvarchar(256) NOT NULL,
    [Price] DECIMAL(10, 2) NOT NULL,
    [Total] DECIMAL(10, 2) NOT NULL,
    CONSTRAINT [PK_InvoiceLine] PRIMARY KEY ([InvoiceId], [ParkingSpaceId]),
    CONSTRAINT [FK_InvoiceLine_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_InvoiceLine_ParkingSpaces_ParkingSpaceId] FOREIGN KEY ([ParkingSpaceId]) REFERENCES [ParkingSpaces] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [InvoicePayment] (
    [InvoiceId] int NOT NULL,
    [PaymentId] int NOT NULL,
    CONSTRAINT [PK_InvoicePayment] PRIMARY KEY ([InvoiceId], [PaymentId]),
    CONSTRAINT [FK_InvoicePayment_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_InvoicePayment_Payments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [Payments] ([Id]) ON DELETE NO ACTION
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'Email', N'FirstName', N'Gender', N'IsActive', N'LastName', N'MiddleName', N'ModifiedBy', N'NormalizedEmail', N'PhoneNumber') AND [object_id] = OBJECT_ID(N'[Customers]'))
    SET IDENTITY_INSERT [Customers] ON;
INSERT INTO [Customers] ([Id], [CreatedBy], [Email], [FirstName], [Gender], [IsActive], [LastName], [MiddleName], [ModifiedBy], [NormalizedEmail], [PhoneNumber])
VALUES (1, 1, NULL, N'Alice', N'F', NULL, N'Smite', NULL, 1, NULL, N'555-555-5555');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'Email', N'FirstName', N'Gender', N'IsActive', N'LastName', N'MiddleName', N'ModifiedBy', N'NormalizedEmail', N'PhoneNumber') AND [object_id] = OBJECT_ID(N'[Customers]'))
    SET IDENTITY_INSERT [Customers] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description') AND [object_id] = OBJECT_ID(N'[InvoiceStatuses]'))
    SET IDENTITY_INSERT [InvoiceStatuses] ON;
INSERT INTO [InvoiceStatuses] ([Id], [Description])
VALUES (1, N'awaiting payment'),
(2, N'bad debt'),
(3, N'draft'),
(4, N'paid'),
(5, N'past due'),
(6, N'partial'),
(7, N'void');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description') AND [object_id] = OBJECT_ID(N'[InvoiceStatuses]'))
    SET IDENTITY_INSERT [InvoiceStatuses] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'SpaceType') AND [object_id] = OBJECT_ID(N'[ParkingSpaceTypes]'))
    SET IDENTITY_INSERT [ParkingSpaceTypes] ON;
INSERT INTO [ParkingSpaceTypes] ([Id], [SpaceType])
VALUES (3, N'house'),
(2, N'mobile_home_space'),
(1, N'rv_space');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'SpaceType') AND [object_id] = OBJECT_ID(N'[ParkingSpaceTypes]'))
    SET IDENTITY_INSERT [ParkingSpaceTypes] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Method') AND [object_id] = OBJECT_ID(N'[PaymentMethods]'))
    SET IDENTITY_INSERT [PaymentMethods] ON;
INSERT INTO [PaymentMethods] ([Id], [Method])
VALUES (1, N'cash'),
(2, N'check'),
(3, N'credit'),
(4, N'debit'),
(5, N'money_order');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Method') AND [object_id] = OBJECT_ID(N'[PaymentMethods]'))
    SET IDENTITY_INSERT [PaymentMethods] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AgreementType') AND [object_id] = OBJECT_ID(N'[RentalAgreementTypes]'))
    SET IDENTITY_INSERT [RentalAgreementTypes] ON;
INSERT INTO [RentalAgreementTypes] ([Id], [AgreementType])
VALUES (1, N'daily'),
(2, N'monthly'),
(3, N'weekly');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AgreementType') AND [object_id] = OBJECT_ID(N'[RentalAgreementTypes]'))
    SET IDENTITY_INSERT [RentalAgreementTypes] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amps', N'Comments', N'CreatedBy', N'Description', N'ModifiedBy', N'Name', N'ParkingSpaceTypeId', N'RecurringRate') AND [object_id] = OBJECT_ID(N'[ParkingSpaces]'))
    SET IDENTITY_INSERT [ParkingSpaces] ON;
INSERT INTO [ParkingSpaces] ([Id], [Amps], [Comments], [CreatedBy], [Description], [ModifiedBy], [Name], [ParkingSpaceTypeId], [RecurringRate])
VALUES (1, 30, NULL, 1, NULL, 1, N'1', 1, 400.0),
(30, 30, NULL, 1, NULL, 1, N'30', 1, 400.0),
(29, 30, NULL, 1, NULL, 1, N'29', 1, 400.0),
(28, 30, NULL, 1, NULL, 1, N'28', 1, 400.0),
(27, 30, NULL, 1, NULL, 1, N'27', 1, 400.0),
(26, 30, NULL, 1, NULL, 1, N'26', 1, 400.0),
(25, 30, NULL, 1, NULL, 1, N'25', 1, 400.0),
(24, 30, NULL, 1, NULL, 1, N'24', 1, 400.0),
(23, 30, NULL, 1, NULL, 1, N'23', 1, 400.0),
(22, 30, NULL, 1, NULL, 1, N'22', 1, 400.0),
(21, 30, NULL, 1, NULL, 1, N'21', 1, 400.0),
(20, 30, NULL, 1, NULL, 1, N'20', 1, 400.0),
(19, 30, NULL, 1, NULL, 1, N'19', 1, 400.0),
(18, 30, NULL, 1, NULL, 1, N'18', 1, 400.0),
(17, 30, NULL, 1, NULL, 1, N'17', 1, 400.0),
(16, 30, NULL, 1, NULL, 1, N'16', 1, 400.0),
(15, 30, NULL, 1, NULL, 1, N'15', 1, 400.0),
(14, 30, NULL, 1, NULL, 1, N'14', 1, 400.0),
(13, 30, NULL, 1, NULL, 1, N'13', 1, 400.0),
(12, 30, NULL, 1, NULL, 1, N'12', 1, 400.0),
(11, 30, NULL, 1, NULL, 1, N'11', 1, 400.0),
(10, 30, NULL, 1, NULL, 1, N'10', 1, 400.0),
(9, 30, NULL, 1, NULL, 1, N'9', 1, 400.0),
(8, 30, NULL, 1, NULL, 1, N'8', 1, 400.0),
(7, 30, NULL, 1, NULL, 1, N'7', 1, 400.0),
(6, 30, NULL, 1, NULL, 1, N'6', 1, 400.0),
(5, 30, NULL, 1, NULL, 1, N'5', 1, 400.0),
(4, 30, NULL, 1, NULL, 1, N'4', 1, 400.0),
(3, 30, NULL, 1, NULL, 1, N'3', 1, 400.0),
(2, 30, NULL, 1, NULL, 1, N'2', 1, 400.0),
(31, 30, NULL, 1, NULL, 1, N'31', 1, 400.0),
(32, 30, NULL, 1, NULL, 1, N'32', 1, 400.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amps', N'Comments', N'CreatedBy', N'Description', N'ModifiedBy', N'Name', N'ParkingSpaceTypeId', N'RecurringRate') AND [object_id] = OBJECT_ID(N'[ParkingSpaces]'))
    SET IDENTITY_INSERT [ParkingSpaces] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'ModifiedBy', N'ParkingSpaceId', N'RecurringDueDate', N'RentalAgreementTypeId', N'TerminatedOn') AND [object_id] = OBJECT_ID(N'[RentalAgreements]'))
    SET IDENTITY_INSERT [RentalAgreements] ON;
INSERT INTO [RentalAgreements] ([Id], [CreatedBy], [ModifiedBy], [ParkingSpaceId], [RecurringDueDate], [RentalAgreementTypeId], [TerminatedOn])
VALUES (1, 1, 1, 1, 1, 2, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedBy', N'ModifiedBy', N'ParkingSpaceId', N'RecurringDueDate', N'RentalAgreementTypeId', N'TerminatedOn') AND [object_id] = OBJECT_ID(N'[RentalAgreements]'))
    SET IDENTITY_INSERT [RentalAgreements] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CustomerId', N'RentalAgreementId') AND [object_id] = OBJECT_ID(N'[CustomerRentalAgreements]'))
    SET IDENTITY_INSERT [CustomerRentalAgreements] ON;
INSERT INTO [CustomerRentalAgreements] ([CustomerId], [RentalAgreementId])
VALUES (1, 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CustomerId', N'RentalAgreementId') AND [object_id] = OBJECT_ID(N'[CustomerRentalAgreements]'))
    SET IDENTITY_INSERT [CustomerRentalAgreements] OFF;

GO

CREATE INDEX [IX_CustomerRentalAgreements_RentalAgreementId] ON [CustomerRentalAgreements] ([RentalAgreementId]);

GO

CREATE INDEX [IX_Customers_FirstName_LastName] ON [Customers] ([FirstName], [LastName]);

GO

CREATE INDEX [IX_CustomerVehicle_CustomerId] ON [CustomerVehicle] ([CustomerId]);

GO

CREATE INDEX [IX_InvoiceLine_ParkingSpaceId] ON [InvoiceLine] ([ParkingSpaceId]);

GO

CREATE INDEX [IX_InvoicePayment_PaymentId] ON [InvoicePayment] ([PaymentId]);

GO

CREATE INDEX [IX_Invoices_InvoiceStatusId] ON [Invoices] ([InvoiceStatusId]);

GO

CREATE INDEX [IX_Invoices_RentalAgreementId] ON [Invoices] ([RentalAgreementId]);

GO

CREATE UNIQUE INDEX [IX_ParkingSpaces_Name] ON [ParkingSpaces] ([Name]);

GO

CREATE INDEX [IX_ParkingSpaces_ParkingSpaceTypeId] ON [ParkingSpaces] ([ParkingSpaceTypeId]);

GO

CREATE INDEX [IX_Payments_CustomerId] ON [Payments] ([CustomerId]);

GO

CREATE INDEX [IX_Payments_PaymentMethodId] ON [Payments] ([PaymentMethodId]);

GO

CREATE INDEX [IX_RentalAgreements_ParkingSpaceId] ON [RentalAgreements] ([ParkingSpaceId]);

GO

CREATE INDEX [IX_RentalAgreements_RentalAgreementTypeId] ON [RentalAgreements] ([RentalAgreementTypeId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200809191022_InitialCreate', N'3.1.6');

GO

