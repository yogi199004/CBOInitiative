CREATE TABLE [dbo].[UserApplicationLocale] (
    [UserId]              UNIQUEIDENTIFIER NOT NULL,
    [ApplicationLocaleId] INT NOT NULL,
    CONSTRAINT [PK_UserApplicationLocale] PRIMARY KEY CLUSTERED ([UserId] ASC, [ApplicationLocaleId] ASC),
    CONSTRAINT [FK_UserApplicationLocale_ApplicationLocale] FOREIGN KEY ([ApplicationLocaleId]) REFERENCES [dbo].[ApplicationLocale] ([Id]),
    CONSTRAINT [FK_UserApplicationLocale_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

