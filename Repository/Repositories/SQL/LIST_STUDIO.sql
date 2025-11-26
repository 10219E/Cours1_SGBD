SELECT
    [UID] AS UID,
    [No]         AS StudioNo,
    [Building] AS Building,
    [Lease Renewal] AS lease,
    [Student] AS id,
    [First Name] AS fname,
    [Last Name]  AS lname,
    [E-mail]     AS email,
    [Mobile]      AS phone,
    [Confirmed]  AS confirmed,
    [Section]    AS section
FROM dbo.Studio
JOIN dbo.Students ON Students.id = Studio.Student
ORDER BY Building;