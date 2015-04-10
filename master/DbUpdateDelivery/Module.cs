﻿namespace DbUpdateDelivery
{
    public class Module
    {

        public const string ConnectionString =
            @"Data Source=JJ-SERVER\DRUGHOUSESERVER; Initial Catalog=DrugHouse; Integrated Security=SSPI";

        public const string SqlText = @"
DECLARE @CurrentMigration [nvarchar](max)

IF object_id('[dbo].[__MigrationHistory]') IS NOT NULL
    SELECT @CurrentMigration =
        (SELECT TOP (1) 
        [Project1].[MigrationId] AS [MigrationId]
        FROM ( SELECT 
        [Extent1].[MigrationId] AS [MigrationId]
        FROM [dbo].[__MigrationHistory] AS [Extent1]
        WHERE [Extent1].[ContextKey] = N'DrugHouse.Model.Repositories.DrugHouseContext'
        )  AS [Project1]
        ORDER BY [Project1].[MigrationId] DESC)

IF @CurrentMigration IS NULL
    SET @CurrentMigration = '0'

IF @CurrentMigration < '201503251259593_InitialCreate'
BEGIN
    CREATE TABLE [dbo].[Drug] (
        [DrugId] [bigint] NOT NULL IDENTITY,
        [Name] [nvarchar](max),
        [DrugType] [int] NOT NULL,
        [Remarks] [nvarchar](max),
        [Discriminator] [nvarchar](128) NOT NULL,
        CONSTRAINT [PK_dbo.Drug] PRIMARY KEY ([DrugId])
    )
    CREATE TABLE [dbo].[MedicalPractitioner] (
        [Id] [bigint] NOT NULL IDENTITY,
        [EducationDegree] [nvarchar](max),
        [Name] [nvarchar](max),
        [Gender] [int] NOT NULL,
        [Age] [int] NOT NULL,
        [Address] [nvarchar](max),
        [Email] [nvarchar](max),
        [PhoneNumber] [nvarchar](max),
        [GuardianName] [nvarchar](max),
        [GuardianRelationship] [int] NOT NULL,
        [Remark] [nvarchar](max),
        [Location_Id] [bigint],
        CONSTRAINT [PK_dbo.MedicalPractitioner] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_Location_Id] ON [dbo].[MedicalPractitioner]([Location_Id])
    CREATE TABLE [dbo].[SimpleEntity] (
        [Id] [bigint] NOT NULL IDENTITY,
        [Name] [nvarchar](max),
        [Marker] [nvarchar](max),
        [Discriminator] [nvarchar](128) NOT NULL,
        CONSTRAINT [PK_dbo.SimpleEntity] PRIMARY KEY ([Id])
    )
    CREATE TABLE [dbo].[PatientAdmitance] (
        [Id] [bigint] NOT NULL IDENTITY,
        [Date] [datetime] NOT NULL,
        [DoctorFee] [decimal](18, 2) NOT NULL,
        [Patient_Id] [bigint],
        [PrimaryDiagnosis_Id] [bigint],
        [Status_Id] [int],
        CONSTRAINT [PK_dbo.PatientAdmitance] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_Patient_Id] ON [dbo].[PatientAdmitance]([Patient_Id])
    CREATE INDEX [IX_PrimaryDiagnosis_Id] ON [dbo].[PatientAdmitance]([PrimaryDiagnosis_Id])
    CREATE INDEX [IX_Status_Id] ON [dbo].[PatientAdmitance]([Status_Id])
    CREATE TABLE [dbo].[Patient] (
        [Id] [bigint] NOT NULL IDENTITY,
        [Name] [nvarchar](max),
        [Gender] [int] NOT NULL,
        [Age] [int] NOT NULL,
        [Address] [nvarchar](max),
        [Email] [nvarchar](max),
        [PhoneNumber] [nvarchar](max),
        [GuardianName] [nvarchar](max),
        [GuardianRelationship] [int] NOT NULL,
        [Remark] [nvarchar](max),
        [Discriminator] [nvarchar](128) NOT NULL,
        [Location_Id] [bigint],
        CONSTRAINT [PK_dbo.Patient] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_Location_Id] ON [dbo].[Patient]([Location_Id])
    CREATE TABLE [dbo].[PatientVisit] (
        [Id] [bigint] NOT NULL IDENTITY,
        [Date] [datetime] NOT NULL,
        [Complaints] [nvarchar](max),
        [History] [nvarchar](max),
        [Observation] [nvarchar](max),
        [Treatment] [nvarchar](max),
        [DoctorFee] [decimal](18, 2) NOT NULL,
        [DrugFee] [decimal](18, 2) NOT NULL,
        [RestDuration] [nvarchar](max),
        [PrimaryDiagnosis_Id] [bigint],
        [SecondaryDiagnosis_Id] [bigint],
        [Status_Id] [int],
        [Patient_Id] [bigint],
        CONSTRAINT [PK_dbo.PatientVisit] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_PrimaryDiagnosis_Id] ON [dbo].[PatientVisit]([PrimaryDiagnosis_Id])
    CREATE INDEX [IX_SecondaryDiagnosis_Id] ON [dbo].[PatientVisit]([SecondaryDiagnosis_Id])
    CREATE INDEX [IX_Status_Id] ON [dbo].[PatientVisit]([Status_Id])
    CREATE INDEX [IX_Patient_Id] ON [dbo].[PatientVisit]([Patient_Id])
    CREATE TABLE [dbo].[Prescription] (
        [Id] [bigint] NOT NULL IDENTITY,
        [Remark] [nvarchar](max),
        [PrescriptionType] [nvarchar](max),
        [Discriminator] [nvarchar](128) NOT NULL,
        [Drug_DrugId] [bigint],
        [PatientVisit_Id] [bigint],
        CONSTRAINT [PK_dbo.Prescription] PRIMARY KEY ([Id])
    )
    CREATE INDEX [IX_Drug_DrugId] ON [dbo].[Prescription]([Drug_DrugId])
    CREATE INDEX [IX_PatientVisit_Id] ON [dbo].[Prescription]([PatientVisit_Id])
    CREATE TABLE [dbo].[PatientStatus] (
        [Id] [int] NOT NULL IDENTITY,
        [Status] [nvarchar](max),
        CONSTRAINT [PK_dbo.PatientStatus] PRIMARY KEY ([Id])
    )
    ALTER TABLE [dbo].[MedicalPractitioner] ADD CONSTRAINT [FK_dbo.MedicalPractitioner_dbo.SimpleEntity_Location_Id] FOREIGN KEY ([Location_Id]) REFERENCES [dbo].[SimpleEntity] ([Id])
    ALTER TABLE [dbo].[PatientAdmitance] ADD CONSTRAINT [FK_dbo.PatientAdmitance_dbo.Patient_Patient_Id] FOREIGN KEY ([Patient_Id]) REFERENCES [dbo].[Patient] ([Id])
    ALTER TABLE [dbo].[PatientAdmitance] ADD CONSTRAINT [FK_dbo.PatientAdmitance_dbo.SimpleEntity_PrimaryDiagnosis_Id] FOREIGN KEY ([PrimaryDiagnosis_Id]) REFERENCES [dbo].[SimpleEntity] ([Id])
    ALTER TABLE [dbo].[PatientAdmitance] ADD CONSTRAINT [FK_dbo.PatientAdmitance_dbo.PatientStatus_Status_Id] FOREIGN KEY ([Status_Id]) REFERENCES [dbo].[PatientStatus] ([Id])
    ALTER TABLE [dbo].[Patient] ADD CONSTRAINT [FK_dbo.Patient_dbo.SimpleEntity_Location_Id] FOREIGN KEY ([Location_Id]) REFERENCES [dbo].[SimpleEntity] ([Id])
    ALTER TABLE [dbo].[PatientVisit] ADD CONSTRAINT [FK_dbo.PatientVisit_dbo.SimpleEntity_PrimaryDiagnosis_Id] FOREIGN KEY ([PrimaryDiagnosis_Id]) REFERENCES [dbo].[SimpleEntity] ([Id])
    ALTER TABLE [dbo].[PatientVisit] ADD CONSTRAINT [FK_dbo.PatientVisit_dbo.SimpleEntity_SecondaryDiagnosis_Id] FOREIGN KEY ([SecondaryDiagnosis_Id]) REFERENCES [dbo].[SimpleEntity] ([Id])
    ALTER TABLE [dbo].[PatientVisit] ADD CONSTRAINT [FK_dbo.PatientVisit_dbo.PatientStatus_Status_Id] FOREIGN KEY ([Status_Id]) REFERENCES [dbo].[PatientStatus] ([Id])
    ALTER TABLE [dbo].[PatientVisit] ADD CONSTRAINT [FK_dbo.PatientVisit_dbo.Patient_Patient_Id] FOREIGN KEY ([Patient_Id]) REFERENCES [dbo].[Patient] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [FK_dbo.Prescription_dbo.Drug_Drug_DrugId] FOREIGN KEY ([Drug_DrugId]) REFERENCES [dbo].[Drug] ([DrugId])
    ALTER TABLE [dbo].[Prescription] ADD CONSTRAINT [FK_dbo.Prescription_dbo.PatientVisit_PatientVisit_Id] FOREIGN KEY ([PatientVisit_Id]) REFERENCES [dbo].[PatientVisit] ([Id]) ON DELETE CASCADE
    CREATE TABLE [dbo].[__MigrationHistory] (
        [MigrationId] [nvarchar](150) NOT NULL,
        [ContextKey] [nvarchar](300) NOT NULL,
        [Model] [varbinary](max) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
        CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
    )
    INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
    VALUES (N'201503251259593_InitialCreate', N'DrugHouse.Model.Repositories.DrugHouseContext',  0x1F8B0800000000000400ED5D596FE4B8117E0F90FF20F4D36E32EBB63D8B6062D8BBF0FAC81819DB03F7CC266F06DDA2DBC2EAE8E818D808F2CBF2909F94BF10EA6CDE9728B53A680C30704B6491ACFA58ACA258ACFFFEFB3FA73FBF46A1F70DA65990C467B3A383C39907E365E207F1EA6C56E4CF3F7C98FDFCD3EF7F777AE547AFDEAF6DB9F765395433CECE662F79BE3E99CFB3E50B8C40761005CB34C992E7FC60994473E027F3E3C3C33FCF8F8EE6109198215A9E77FA50C47910C1EA07FA7991C44BB8CE0B10DE263E0CB3E6397AB3A8A87A772082D91A2CE1D9EC322D561F9322830755D98307B84EB2204FD2006633EF3C0C00EAD30286CF330FC47192831CF5F8E46B0617799AC4ABC51A3D00E197B73544E59E4198C16624279BE2BA833A3C2E0735DF546C492D8B2C4F22438247EF1B2ECDE9EA56BC9E755C447CBC42FCCEDFCA5157BCACD938F3E8864E2EC2B42CC4B2B962D841F9F49DB779F70BC8E0BB0E16083DE5BF77DE4511E6450ACF6258E42908DF799F8BA73058FE15BE7D497E83F1595C8421DE3DD441F48E78801E7D4E93354CF3B707F88C75FAC69F7973B2EE9CAEDC55A5EAD543BB89F33FFD38F3EE5027C053083B14606C58203CC1BFC018A62087FE6790E7304542BCF161C547A607547BE5FF6D6B0876682ECDBC5BF0FA09C6ABFCE56C86FE9C79D7C12BF4DB274D0FBEC6019A7AA8529E1650D54839A8B289AE2184F983CD436678726A0F0850E96F99E35E9FCE37C053C2F12A5A97ACB5C16455D51530BDB232C5536A584CFF6FA11F2C41F839054BF406B50953F39170886C71B2D94CB43127D9955F2C2B42977095C2E1E7DB28931AB1C32FA183C1AF7E6435A9CF571093C7FB637302BE9FC2CCB556E0C832024138782B9F5FD094BA2BA2278CC38309B200A91F80781CD4348D3DC0B09A11D94BB0263084BFE8B13CB81FC81DF816ACAAAE510D7E4A968DE983F7BD190E47513E6E2A5CA749F490847CADDC957B5C2445BA2C65936814FE02D215CC2D17B74510AD4378D56835D35501AFBD5F0EB6AB9E6FD12470AE3CAC7064692C312486329A48CCCB8D2724EB0009F9DC8F821CC4E59C341D164D613F4F846D5DA25A6D6BE5DF5F82C87C39B84C96A817D71BABEB122E830820187C4ED15FCD66C28799B7401A15BDD6B03C842B41235BEE42D0BC7BEC049FE1EA9F7DDBE9F156E9738AB4EB8276FF5234F2F4ED3200AB38C9824CD6D1AE9947B616D36F716166ED52D7E02D60B2512D10DA0ACDB1B4652523A88BA8FBDD94EBB5DC7680B1D4227BE5216C6BEF03ED7DA0BD0F34960F84AF6A2E163F81F6E5AD8F4EDCB4963ECF35A3DF093B2775C1645DFB159921B99C736D11B65BF51B61A79AD72E96A98A94F55A55D5DE2F58C2B69C58BB1709F2654010E7C32F021F030483F46DF076EE9F32987E6B66E5C06D7D4921C8A3CA221BFA93887BB784F3D16548FA0F30CB2F8B7420C1F4F2B194CA52E45B51BA54DFAF82D9320DD65577643DABE83F52C5995E724A89D43BAFA8E9EA63E215B6ED293D427E41D530FA7A827099C4BED958787504A3618BCAC7C329EFDEB76D9A12F9B5F86B456F5DF8B318102D0C05ACF6DE509028DE618C7B8952233EDE0FAFE3EBCFD83CCC637D7AAC4B6188A75FB278674A18EB4AC2121E42D10B9626D99AD07BAA5AEED0332486DAA127D58AD60E7DAB10F5C7D454BC843908C2CE6BA9E9ECB236E2EE23B9D3462DA3B7F39DA991922D80EBDA7FC4A90C86E1D66EA5E05B44D4D9A25AD7DE64D721586DCEFDE90C6DF10252E81F9434616D9767DD292B7C587539EB817D2D3738C33724671C6324486E61B96DD77A5525F4D0C87F0561817E1E3290224A2FDED262DD153E9217BE00EBAC086157FC585EFC2E893765DFB398AB85211110BEBBEB4A441B9ADB14D22DC0D8A810D135B240B0D20A19DD25F9628D7CCEE700FA224169319FDD167525029AF23605F1B7E019DE3F6BCF9624C60A2B2471098AD50B52EF580DC584F958644F20F6B10AEF0D66D88F3A423ECFB2641954EC179F50DC6CCC926D5FC5BE67728066A38FB9C7206F9148833512225A65CE667F6086AAD95C67C3890E1990ED1C1E1C1CD19CC2B8226716676B5DD467D93E3B67A1527552459BC305F6F88494E3166C50E244BC9BAF64810C0FC26F00A36080718B84DD14FB482273DB800742F78A39A2EC7E02F03C2E85B0A4FB6C0C1A1A9F8FD373DA88BF8F2F213278A077BEAC232A2E40B6043EEB00237DE85BF49137B1F44566CF5A6AD34FB3E7A21D403D066BCC39C5E6E1884A58B8A3A83502D9F6A26366497626C76757ED40EAF59BDAC774CD16720B9425DF3ABB03ACDFCD370BD5FA427FEDD55BB787D450CC37152BA198734C72404DD1639DD36AD6368B61835B9876CC6938EDDE2BA79F1B260D3F0D6BE65E24710E8278E31EB53E5FF902BEF28E3A7C452F6B97326BF6ADE85195841730C748A28E6EF6AD84815A9CCA1C1F83478BEBC928486318AB825119A2E243D11C6AB40479045984E81195D0D225D17E0E16D169F4938A18B999CF12136E528B7BD60259D4B3F63D450D43B5142DD87126AC8A4E640ABD6018FAD9DD7045209E5B36D0EA04AC011ACAF40A46324B8391BCC36A2CFB547EB7AEE78D8D648378097B248E364B0A1F8233BEC8402577C3F51C715B8E8C0B12F64B24871B72BF5CD333C7F941EA20195384BE3846AE599D5C2183FBD1538811A5C36EEAB2B3B86915BF1A3C32375B97FFFD19479FAA51F24E66F75AF8E42E3828B06F4750D8E2F33C0A3EAA1C762B97BD3F2F254EFA78DC6C0C101507398E84B617EF8053A4C3C01214D851F66B60DB43F10AC8F3E7753C7ADBD58FF2C275996ACF03598098902DBA4EBBA5DB6E6201593AEA234C3D365C4D839FBA5350E8CDBBE39DFBE9D89E07E93CF6EEDDE9BCBE77A979703A175CD0747A0BD6EB205E611736354FBC457D5BD3C50F0BF3CB8BA29AC67C49709EDE5FE85ACA9314AC20F5B63CB5EFC3EB20CDF24B908327507EC9BEF023A618B33F21F031DBE6F02D08567CADC7D9962EFF6E8E0F65E5DFF7CFDFC92EB0AAB630BEA7E96ED87A8D465A9EA4AF060D29A393ADE69517688110A482FB902E92B08862F2198D4931953AAC0BA7513FD1A7B0399943F7A47EAA4FA9BBBE0827D43D64E99CCE298ED2A29C33B23411B652C6F5D9AAC9C919CD01BFDA1E680F1B60F732114482D2628F8218A0FE4D91BF3BC2DA91B9DABEDFA8332D65C7DD6D72262EDE4EABB9F4B4A88884490BD24C0F321751E1A4989763EAD736921AA7D13ED3A7524550E324AA0706F5DB006A8246FBD080CF758034C1DDFA913E0D22FC19A744BC30E03011E24CF09978634E918C63E651264B98AE95BCA572423A877203DC5B5AF8B7181B8B8BF8963392A6E9AF11DA0B8C701AEDB369ADE2EC6547939311B3A4732E68DA19AB49CEAB49B37A37AC28D6FF77263BE633B0B9FCD42486516975803E21BFEA8901854DD0374166F3786A101862396B48DBAC64ED87FF9D59C4F666EDDEACDDB659EB405DDB5A35C3CD57669525230177C69611726882BCDD29E345F8FDAD9FA0EAB362D6D212549FAAC1825F2784D3C19FEB53EB6E0CC249750FF5E9103702E1B48817FAF4B05B7F706AD8E3614D3CD9C62D4BA97D68B2DCE037F5908B0EFE663A53587128A8B7058AD1B73243F1A39E234D66339B41687D31977A102618F376628B257367C5E464C7AE9DEC3D1BBB639C4879356956EF94AD223CBBD1CF56E193D5375644F5875170EDA1109C02FFA0C81802238FA2283F326287A2EDBF2562442CBE19962772E82D748DA3F42C67B564DB91E109B90A1B6B09F4EC66733CC9B29BC6BDDB28959BACBC35A7BB5A4273F0F421266370F1A20FCCA63D51D574F394231E71F082A5505A820EA0238E86981460C4237686176305C456D4DC6190886182CA45146632297C0CAA50D848150BF3AFA96962E2F184218A4AD19486E828D3A3E83C93295844413893428B64C4AED4093794C762A78C26D1173FCA28A07E4B524DD89DD69184154D0A518AE1BB46151D2E60072C9A8AC996AB5A64FCE3FFB6F0A2C9B987183F22625A285373C12DD278B1603658E3D171893671A497A5FC38045D234E1CFA3629CC6971C231EAB01B518D91D6D4758A2E221CC716513511E728226290A6851CD1889DB96C6D449EA1C3D6567383112AFE703A2E3D19D03829680CEFCECB822B2D4FDAE95B4C169B43EAC0C9E9594EEA78D269816E3CEB890D40B5C59C623DEB85B469AE6BA2F8DA4961A9C7DAC684E2D245BA2F33CD93EE77178ADB84C112F1B9153FCA68DB8A0F5913924BC7C5D645CA1C46C9B7C02F6362176F590EA383B2C0C1E21FE145581F546A0BDC82387886595EDFC83C3B3E3C3A9E79E76100B23A8EBA89F83DA12FFAD20A013E7A5F8600433F9AD3D5CD03894B2A59E6132902D8EBEE3977BB6A2511106D16A91209B4F5EA9BA99E82555032579931C0309354F97FDB46FC0DA4CB17907E1781D7EF714A5A49B5BA70DC9A58D55BABD489999BFE909F9D498A449284A3E30F8A9E6AA744507E2A1C23EFC450506162141D48C919FCC86CB036E0C332C05A5527F3BFF61A0C91E3B517254E1ED77E6CE6E46A754290978FD55E8538E91AF161889C5ACCDD9437087DAF67B37F56554FBC9BBF3F62B5DF79F72902E78977E8FD4BDE096D45238EA3DA650DE30C556DACE2AEAE2372137D97458C675AF5D1DFB94DA6552681A7EF3AC126BECB603CF53795F5673EA70B3C9FD3BC2F2C953E9DC29C174C476BF4A3AB388032E49E95D8E549B2B78A78B4F656D114AC2297EBE16ED95CE23DFE5DD6354E166436F5792F8C51E9CD7BD1E2A430EF458F4953DE6F360D6EC950A9C89DD3E7A522EFA79C2769F7F03F651B778B4B67570CB20998C976D9B0FF6F94B5C3755C94627AC2C60171F6D2186E586D0790DF1CD7B3C57D4B61384B859B626604F40F027D32D7B1014A455C7394FB92BC61A8EE9D452A21822637AB0C46DA2A7F26922E4C4BE683F0027914795ADA6B2C14827819AC41C81B1DFB19550739A5043AB2F49B4BB82E5DD438978C5CA75945D442D70C8569154F06CD05EA2EF32796D88725699017CA0A22E25B349CA343E3022BB64DF9F1A81191B115556280B51D551926F89B809AD04E17BB091732CADE4A4A9F88CAC621609466D60A0782FB95750F489861401E2CCF6F5312B8349E56B04D9D3B852CB95B0497E24620F74AC614601A414DE3834C3355E780EB901976777B45D207A7E641DBB10163904C790F999121A31D65343A68B4F2DA92DEBC4D7ED9C9404575E1CB04B0223B673DA203A497667B1289B5B789A44962682A5EB445E6F101D7A6AD6EBC6C698D32DC7D999A69639A817D98956A0A3B76A3AF5866C81977D5BA22036BBA730D7472385AAE4D54151E2322CA385F07D09CCDFCA70449BDFE5C619B8B5E37153DAFCD9E39EBDF9419EB798DF6CA69AF95D29ED72A5B4AAF6559DE7B493BBAE41B2342D446F35AD2505342D51AB10FC3B646BCE6B64694D01B5BAB154583EBA212C5A313A4A8E4A6ED945FE5C7E6EED4FFC627B121BAA1CB71ED29BFE961847466A57DCE5CFC5A3A71DE5C69D211C64CC73A2FC0BF62E563098827AAFDD06568907F9A718C0003C6D90C97BD308C335EF9D707CEF707ACAFBCB54AB2218C8F52A647269FF25E98D0798ACC182E8DBDB30960C6D4A9E7A4DF55B6B84D2E4FAEFAEA7CD3DB6480C304F2B68BE058431D2F4FBCB36930B67D3068C27717D3C239430C12BAB3B7452017BB88CB1881FAD725CC82D586447903460C978473DD95B9899F93D6CDA77AD416A1C338610E7CE4799FA779F08CEC62F41AD9A65910233BA4B977FE2A7A82FE4D7C5FE4EB22474386D15348E0ACDC2B90B55F65AD27FB7C7A5FDB072E8680BA19946115F7F12F4510FA5DBFAF39D1A10212E526447390B294655E1EA85CBD7594EE90ABA047A8615FB777F20596511B39CCEEE305F8066DFAF635839FE00A2CDFDA4B3FC444D48220D97E5AAA9D1444594363531FFD4418F6A3D79FFE0714C355039BCA0000 , N'6.1.1-30610')
END

IF @CurrentMigration < '201504071441571_AddedDrugCount'
BEGIN
    ALTER TABLE [dbo].[Prescription] ADD [DrugCount] [int] NOT NULL DEFAULT 0
    ALTER TABLE [dbo].[Prescription] ADD [Dosage] [nvarchar](max)
    DECLARE @var0 nvarchar(128)
    SELECT @var0 = name
    FROM sys.default_constraints
    WHERE parent_object_id = object_id(N'dbo.Prescription')
    AND col_name(parent_object_id, parent_column_id) = 'PrescriptionType';
    IF @var0 IS NOT NULL
        EXECUTE('ALTER TABLE [dbo].[Prescription] DROP CONSTRAINT [' + @var0 + ']')
    ALTER TABLE [dbo].[Prescription] DROP COLUMN [PrescriptionType]
    INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
    VALUES (N'201504071441571_AddedDrugCount', N'DrugHouse.Model.Repositories.DrugHouseContext',  0x1F8B0800000000000400ED1DD96E24B7F13D40FEA1314F76B2D648BB46E008920D5923C5422C69A1D975F22650D3D4A8E13EC67D2C2404F9B23CE493F20B61DFBCAF66B77A0C618185A64916C9AA621D248BF5BFFFFCF7E487E728F4BEC0340B92F874717470B8F060BC49FC20DE9E2E8AFCF19BEF163F7CFFC73F9C5CF8D1B3F74B5BEF43590FB58CB3D3C5539EEF8E97CB6CF30423901D44C1264DB2E4313FD824D112F8C9F2FDE1E15F9747474B88402C102CCF3BB92BE23C8860F503FD3C4FE20DDCE50508AF131F8659F31D95AC2BA8DE0D8860B6031B78BA58A5C5F6A7A4C8E04155F7E00EEE922CC8933480D9C23B0B0380C6B486E1E3C203719CE42047233EFE9CC1759E26F176BD431F40F8E9650751BD471066B099C9715F5D775287EFCB492DFB862DA84D91E5496408F0E84383A525DDDC0AD78B0E8B088F1708DFF94B39EB0A97351A171EDDD1F179989695583457083B28BFBEF3FAB21F4106DF756C81B8A7FCF7CE3B2FC2BC48E1690C8B3C05E13BEF63F110069BBFC3974FC9AF303E8D8B30C487870688CA880FE8D3C734D9C1347FB9838FD8A0AFFC85B724DB2EE9C65D53AA5D3DB5AB38FFCBB70BEF060D023C84B0E3020C0D6BC44FF06F308629C8A1FF11E4394C1111AF7C58E1911901D55FF97FDB1B623BB49616DE3578FE19C6DBFCE97481FE5C7897C133F4DB2FCD083EC7015A7AA8519E1650D54939A9B28BAE23C4F307FD47667A72687788A1D25F33C7A33E59F68CA764C78B6857A2D68627ABA6AE18D32B1B5338A5A6C58CFF1AFAC106841F53B04125A84F989ACF8403E415179BCD429B72915DF8C5A602B482DB148EBFDE2659D4081D7EC93A18FBD59FAC16F5D91662F4F8F0DE1C80EFA730732D1538B48C40108EDECBC727B4A46E8AE801C3F068842C40EA07209E866B9ACEEE6058AD88EC29D8113C84170C500FEE277203BE04DB6A6854873F279BC6F4C1C7DE4C872328EFFB06976912DD25215F2A77F5EED749916E4ADA241A953F81740B734BE5B60EA25D082F1AA966AA15F0D66FEAE075C5F3355A04CE8587151F591A4B0C88B18C2692E7E5C613A27580887CE647410EE2724D9A4E8B86F0B64E847DAD50ABB6B7F2EF4F4164AE0E56C9068DE2B2B7BA5670134400B1C1C714FDD56C267CB7F0D648A2A2620DCB43A8091ADA7215415376DF113EC3C53F5BDAC9F156E873AAB47A417B7C299A79FAB20AC0364EB220930DB4EBE69E6DC58C5B5C99D15DEA163C05269BD51A715BA13997B6AE64067515F5B89B7A83D46DC7309652E44D7808FB7AF381DE7CA0371F682A1F08D76A2E949F40FAF2F4A31337AD85CF73CDE832E1E0A42E986C68BF2033249763AEADC20EAB2E110EAA2976A1A62A50D6BAAA6AFDA6B0847D39B176CF13E4CB8020CEC757023F05880DD297D1FBB97DC860FAA5599523F7F52985208F2A8B6CEC2311F76E09E7D0654CF87730CB57453A126106F9584A6129F2AD2859AAEF57C16C9306BB6A38B29155F0EFA9EACC2839B544E29D57D554FB9878856D7F4A8F905F51358DA19E20DC24B16F36175E1BC16CD8AAF2F970EABBF76D9BAE447E2D5EAC18AD0B7F1663440B43016BFD66284804EF38C63D477F9C2745AF0BED7CD2559281DEAF9D4041D467E0BC0583B1D77D5D0B5B2E7421BB58981AC6829630A3C7D01202BD26532883D7B9E5F63E0362ACED7D5226696DEFB7D2547F4E4DC315CC4110762E4F0D679F451977C1BB13652DA25FE790AAA1922D03D7ADFF8C43198D875BA39762DF22A22E26D5BB4557D96508B6FDA5419DA9AD9F400AFD831226AC8DFAACBBA2854FABAE673DB1CFE5EE68F882E88CF318C924D7B0DCF36B5DB292F5D0CC7F0161817E1E322C45D45EBFA4C5AEAB7C24AF7C0E765911C2AEFA7B256C34ECBEFA0779F59B24EEEB7ECBB2684D3B093DF19D645714ED61BE264DAF01867505452F91B583D55690F426C9D73BE4DF3E06D017D1550BF9EC16AC2B12D0905F9310FF081EE1EDA3F6E24A62ACB282122B506C9F9036C05A28D6D74F45F600621F6BE07C859D6559B2092AF48B6F43F69BC064DF17B1EF995CD6E9C537F7CAE5352269B04344444AE974F12766AA9ADD75269FE84203D9CFE1C1C1118D290C2B726471B6F1456396EDE973F49A6A902AD81C2CB05735A418B74083924FC427074A14C8F84178DE30090F305E94709862974A649D1BE040E88D31D7A1DD2F009E83A62096744F8FE186C645E48C9CB6F96FE31544F611F4CE3675F4C639C836C067FD65240F7D8B31F216963EC9EC514B6D306A8E5CB4DBA887608D35A7D8A89C50080B772FB56620DBCA748C2CC92EE8F4E8AAFD4DBD71537BA6AED1426EB7B2E05BDF7804FDDD9C8FA8F40B7DB2ACA7B7C79450CCF98D1551CC3126B90CA718B1CECD386B9BC5B0C3575876CCCD3BEDD12B979F1B248DBF0C6BE49E27710E82B8778F5A9FAF2C80CFBC6B159F5161ED5266CD36173DAB12F01AE6184834D07E9B4B1814C669CCF13178B0B89E8C0234C66355E02B03547C019B038DA6200F20CB217A4025B07441B447CF22388D7C520123F7FE5960C23D6DF1C85A46168DAC2DA7A0615C2DE516ECEA14D644270A865618867E76375D11132F2D3B686502D601CDCAB4062391A58148DEC538167D2ABF5BD7F3C666D273BC043D12479B05854FC1195E644C2577C3F51C715B8C4CCB24ECC125071B72BF5CD333C7F141CA20195284BE3806AED14EAE38837B462AE411A5C36EEAB2B37CD30A7E35F3C8DC6C5DFC0F471C7D8347893B99DD6BE193BBC0A0C0BE9D40608BEF0E29F0A872D8AD5CF6E1B89438E9D361B331405418E43812DA5EBC034C910E030B506047D9EBC07684620DC8F3E7753C7A5BED4779E1BA48B5C7812C184D88165DA7DDD26D37B1802C1DF509961E1B1AA7814FDD2528F4E6DDE1CEFD726CAF8F741E7B5776B2ACDF786A3E9C2C058F419D5C83DD2E88B7D8E350CD176F5DBF0C75FECDDAFCA1A4A886B1DC1098A7F717BA9EF224055B48959611023EBC0CD22C5F811C3C80F224FBDC8F986ACCFE84C0C76CBBC3B72058F2B51E675BBBFCBBB96D94957FDF3E7E257B2CABDAC2F89A86DBA3F512CDB4BCB55F4D1A524627DBCC2B1FEB022148056F2F9D276111C5E4379A27C550EA10321C46FD451F427F91871E49FD551F52F754120EA8FBC8C239595218A549B9646869426C258DEBAB58B3A3335A037EB53DD05E36C0DE80228004A5C51E053140E39B237EF704B51363B52DEFC59996B0E3EE363923176FA7D59C7A5A5044C4A4096926079947AF70504CE194F2B58DDAC661B4DFF4A154D1DA3888EA8341FB36589B80D17E34C0731D8C4D60B7FEA40F8308B5C6211105061826C2A9093C1325E610C998691E64B286A9AEE4A9CA19C91CCA0D706F69E1673136161771963391A4192E11DAC7927018EDB7796971F661A5D9D18851E99CC7A0F6C66A92E36AD6A8DE0F2B8AF5FF9DD18E390636A79F1AC43822AD7E0C80A05FF5C500421F604E80E93FCF8D05C650670D681B4DD61EFCEF8D127B336BDFCCDAD7366B1D886B5BAB66BCF5CA68593270706F6C1921866688DBBD325E84E76FC30855DF15B3A696A0F95C0D16FCE9221C0EFE5D1F5AF73A110EAAFBA80F87787D08874514E8C3C35E18C2A1619FC735F1641BB72CA4F6A389BAC15F0522950E5E329F25ACB81434D802C5E05B99A1F855CF8916B399CD2063A9E601119AA99ACF26AC5EBF2242F279FD6D664A96791A63763467752EFB9CC7FE1835525CCD1AD57B65E308EF7C0CB371F860F58D1C51FB7104637B990487C0BF603205C1C82B2CCAC349EC32B5FD192406C4E2ACB1BCC9436FBD6B5CC16731AB45DB0E0C8FC855B8590B60E0309B6B4D96C3341E5D2F54AEB2F2719EEE490ACDC9D3979F8C998B17B560B6EC89A6A69BAE1CF288831E2C89D20274C03AE2288A59318C78C6CEF8C55800B10D357726246498A1701185A7CC8A3F4615286C848B85F9D7B43431F178C41045B36852437405EA5E740FCA945944C13BB3E216C98C5D89136E0890C50E1B0D6228FF28A38786A9A41AB03BA92309479A154729A6EF9AABE830033BC6A2A1986CD5AA49C60F1BB0652F1A9C7B16E34752CC8BCBD45870CB69BC18321B5EE3C171C96DE208314BFA7100BAE63871C8DCAC784E0B138EB90E7B78D598D39AB64EB98B08E3B1E5A81A88732E226297E6C539A2193B73D9DA483E4387AD6DE68647A8B8C5F9B8F46420E4AC58637C775E16946979434FDF62B2D81C52075CCECF7252C7A1CE8BE9A6B39ED8C0555B9E53E8B3419C364FBD268ACB9D152F0DD06D4C082F5DA53B9969BE74BFBB10DE267C9688EBADF05146E95678C89A505E3A9EB6AE52E6594ABE047E194BBB7EC972181D94150ED6BF85E7617DC1A9AD700DE2E0116679FD92F3E2FDE1D1FB8577160620ABE3AF9B48E163FA8130ADD0E1A30F65E830F4A325DDDC3C00B98492653E9189807D559FF326AC56AE02D166912A5F41DBAE7ED1EA21D80625729589090CD39894FFB77DC45F40BA7902E9571178FE1A87A49B56A50EE3AD8155A3B54AEF98B9190F79EC4C422472311CBDFF4E3152EDCC0BCAA3C229D25B8CC52A4C6CA3032A39633F3263AD0DF361596AAD9A93396A074D86C8433B081227D7EC303473F2C93A01C8CB196B2F429C0C8D3818229716F3A6E515E2BEE7D3C5BFAAA6C7DED53FEFB1D6EFBCDB1431E7B177E8FD5B3E086D41238EBFDA6709E38CABDA18C77DD52372137D9F498C6783F5D1DFB94D365826C9A8EF3A0928BECB60BCF4FBC6FA2B9F33049ECF693E1616CA904161CE0B26A335C6D1351C411872EF4AECF32279B38A78B0DEACA23958452EF5E17ED95CE23DFE7D96354E14329B9E7D108F5129D807C1E2A4591F048F49A53E6C358D6EC950E9D29DC3E7A54B1F269C6769F7F08FB28D87C585B32F06D90CCC64BB8CDDBF1B61ED528FD369B06DEC0A3209F68CED0AE2DAA631A762AD1DAC96FEA69FED9269218C67E470B3DA4CB070465935643666032E1561CD51BA4DF251A37A7416D98B0898DC44361868AB949D88BA302D910FC273E48CE46969EAB1AC10C49B600742DEECD813581DCE2929D081A54B5670577AB7712E99B94EB78A8087AE1B8AA755381935FDA8BB64A3582E2116A4412A2A2B16113FDCE19C3B34DECC62FB94DFAC9A90335E459418F0DA9E8A0C13FE9B8198D0CE50DB471A19258C25A94F0474E32C6094D9D68A0F044F3AEBDEAD30E301799C3DBF4F49CCD37452C1365BEF1C12F3BE2273291E21722F644C194C231E6A7A26D3CC0E3AA21E32E3DDFDD648FACCA97947776A8631C8DFFCC63213B38C7680D2E44CA3954A97F4E66D52DACE8655546FC5CC80576457B4277480F4327BCF2297F76B72D22C79682E5EB445B2F31175D3AB6EBCBC928E32DC7D999B69639AF47D1C4D35871DBBC9359619E74CABB52EC8989CEE4A049D8F8EA66B13908587978892DCD7B137A70BFF214154AF8F2BEA12865B48A8DCED7EA6136E2D5E9FDC8A8A2190A292E99B2CE6754AD650F4C6AE03A647B60AAF57B6965ECFE20EA5FDE8826F8C08511F4DB1A4A3A686AA37621F86ED8D28E6F646D4D09B5B2B154593EB021AC5B31364C5E4660A95BF02C8A60BD53FE393D810DDD4E57CED29CFF430403AABD23E4D2FFEA29D3855AF34CF0963A6638317F0BF42F3B100C40BD57EEA326E901FCD38E60003C4D94C977D6B8C335FF9E903E7FC011B2B4F57493684F159CAE4C8E064F1C4FB58CA4CF1F207B5F96E204B30AE009E0332B41350EBEC098FB300CC903A38713BE7E921551277C5BEE7EF022D6EF3D9935A5F9DE2FA3511E03067BDAD129C6AAAD3A5A677B60CA6B60F46CD31EF6259384788410E79F6A109E4621771195E50FF5AC12CD8F620CAC73362B8219CEBAECE55FC98B46E3E35A2B60A1D010A73E023CFFB2CCD83476417A362649B66418CEC90E6C9FA8BE801FA57F16D91EF8A1C4D19460F21C167E55E81ACFF932533E693DBDA3E70310534CCA08CC8B88D7F2C82D0EFC67DC9092C15802837219A8B94252DF3F242E5F6A58374835C053D400DFABABD934FB00CF8C861761BAFC1176833B6CF19FC196EC1E6A57D2F440C444D0812ED27A5D84941943530FAF6E827E2613F7AFEFEFF6F386C5B7ACB0000 , N'6.1.1-30610')
END


";
    }
}
