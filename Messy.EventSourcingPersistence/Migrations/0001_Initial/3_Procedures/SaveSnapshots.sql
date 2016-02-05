CREATE PROCEDURE [$SchemaName$].[SaveSnapshots]
	@Snapshots [$SchemaName$].[SnapshotsTableType] READONLY
AS
BEGIN
	INSERT INTO [$SchemaName$].[Snapshots] SELECT * FROM @Snapshots
END