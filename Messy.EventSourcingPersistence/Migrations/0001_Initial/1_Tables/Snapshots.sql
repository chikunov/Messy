CREATE TABLE [$SchemaName$].[Snapshots]
(
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateName] [nvarchar](255) NOT NULL,
	[Version] [int] NOT NULL,
	[Data] [varbinary](max) NOT NULL
    CONSTRAINT [PK_$SchemaName$.Snapshots] PRIMARY KEY CLUSTERED ([AggregateId], [Version])
);