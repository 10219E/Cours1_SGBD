SELECT
    [ID]         AS id,
    [First Name] AS fname,
    [Last Name]  AS lname,
    [E-mail]     AS email,
    [Mobile]      AS phone,
    [Confirmed]  AS confirmed,
    [Section]    AS section
FROM dbo.Students
WHERE (TRY_CONVERT(int, @search)) = ID
   OR [First Name] LIKE @search
   OR [Last Name] LIKE @search;