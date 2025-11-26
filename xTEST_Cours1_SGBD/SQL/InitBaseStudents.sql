USE [xTEST_SGBD_C];
SET IDENTITY_INSERT [dbo].[Students] ON;

DELETE FROM [dbo].[Students];

INSERT INTO [dbo].[Students] (
  [id],
  [First Name],
  [Last Name],
  [E-mail],
  [Mobile],
  [Section],
  [Confirmed]
) VALUES
(2, 'Amina',     'Hassan',     'ahassan@ep.com',      '+32499123456', 'IT4', '2025-09-12'),
(3, 'Jean',      'Dupont',     'jdupont@ep.com',      '+32477654321', 'IT2', '2025-09-16'),
(4, 'Sofia',     'Jean',  'srodriguez@ep.com',   '+32488765432', 'MA3', '2025-09-16'),
(6, 'Raj',       'Patel',      'rpatel@ep.com',       '+32495876543', 'MA1', '2025-10-03'),
(9, 'Yuki',      'Tanaka',     'ytanaka@ep.com',      '+32476987654', 'MA1', '2025-10-03');

SET IDENTITY_INSERT [dbo].[Students] OFF;