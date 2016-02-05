CREATE TYPE [$SchemaName$].[SnapshotsTableType] AS TABLE 
(
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateName] [nvarchar](255) NOT NULL,
	[Version] [int] NOT NULL,
	[Data] [varbinary](max) NOT NULL
);