DELETE FROM __EFMigrationsHistory WHERE MigrationId LIKE '%AddEventImageUrl%';
ALTER TABLE Events ADD ImageUrl NVARCHAR(MAX) NULL;
