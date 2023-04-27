SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create   function [dbo].[f_base64_to_binary] (
  @data varchar(max),
  @url_safe bit
)
returns varbinary(max)
as
begin
    declare @binary_data varbinary(max)


    if @url_safe = 1
    begin
        select @data = replace(@data, '-', '+')
        select @data = replace(@data, '_', '/')
    end


    select  @binary_data = col

    from    openjson(
                (
                    select col
                    from (select @data col) T
                    for json auto
                )
            ) with(col varbinary(max))


    return @binary_data
end
GO

CREATE FUNCTION [dbo].[f_hash_password](@password NVARCHAR(500))
RETURNS VARBINARY(64)
AS
BEGIN
    DECLARE 
        @hash VARBINARY(64)
    ;

    SELECT @hash = HASHBYTES('SHA2_512', @password);
    RETURN @hash;
END
GO

---------------------
-- HMAC Encryption --
---------------------
/*
	Copyright © 2012 Ryan Malayter. All Rights Reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are
	met:

	1. Redistributions of source code must retain the above copyright
	notice, this list of conditions and the following disclaimer.

	2. Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

	3. The name of the author may not be used to endorse or promote products
	derived from this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY Ryan Malayter "AS IS" AND ANY EXPRESS OR
	IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT,
	INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
	SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
	HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
	STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
	ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
	POSSIBILITY OF SUCH DAMAGE.
*/

/* 
	This function only takes VARBINARY parameters instead of VARCHAR
	to prevent problems with implicit conversion from NVARCHAR to VARCHAR
	which result in incorrect hashes for inputs including non-ASCII characters.
	Always cast @key and @data parameters to VARBINARY when using this function.
	Tested against HMAC vectors for MD5 and SHA1 from RFC 2202
*/

/*
	List of secure hash algorithms (parameter @algo) supported by MSSQL
	version. This is what is passed to the HASHBYTES system function.
	Omit insecure hash algorithms such as MD2 through MD5
	2005-2008R2: SHA1
	2012-2016: SHA1 | SHA2_256 | SHA2_512
*/
create   function [dbo].[f_hmac_encrypt] (
	@key	varbinary(max),
	@data	varbinary(MAX),
	@algo	varchar(20)
)
returns varbinary(64)
as
begin
	declare @ipad	bigint
	declare @opad	bigint
	declare @i		varbinary(64)
	declare @o		varbinary(64)
	declare @pos	integer

	-- SQL 2005 only allows XOR operations on integer types, so use bigint and iterate 8 times
	set @ipad = cast(0x3636363636363636 as bigint) -- constants from HMAC definition
	set @opad = cast(0x5C5C5C5C5C5C5C5C as bigint)

	if len(@key) > 64 -- if the key is greater than 512 bits we hash it first per HMAC definition
		set @key = cast(hashbytes(@algo, @key) as binary (64))
	else
		set @key = cast(@key as binary (64)) -- otherwise pad it out to 512 bits with zeros

	set @pos = 1
	set @i = cast('' AS varbinary(64)) -- initialize as empty binary value

	while @pos <= 57
	begin
		set @i = @i + cast((substring(@key, @pos, 8) ^ @ipad) as varbinary(64))
		set @pos = @pos + 8
	end

	set @pos = 1
	set @o = cast('' as varbinary(64)) -- initialize as empty binary value

	while @pos <= 57
	begin
		set @o = @o + cast((substring(@key, @pos, 8) ^ @opad) as varbinary(64))
		set @pos = @pos + 8
	end

	return hashbytes(@algo, @o + hashbytes(@algo, @i + @data))
end
GO

-----------------------------
-- JSON Web Token Read --
-----------------------------
CREATE   function [dbo].[f_jwt_decode] (
    @token  varchar(max),
    @secret varchar(max)
)
returns varchar(max) -- returns a json string
as
begin
    declare @header varchar(max),
            @payload varchar(max),
            @signature varchar(max),
            @signature_verify varchar(max)


    declare @token_components table (
        token_index int,
        token_value varchar(max)
    )


    insert  @token_components (token_index, token_value)

    select  token_index = row_number() over (order by patindex(value, @token)),
            token_value = value

    from    string_split(@token, '.');


    select @header = token_value from @token_components where token_index = 1
    select @payload = token_value from @token_components where token_index = 2
    select @signature = token_value from @token_components where token_index = 3


    select @signature_verify = dbo.f_to_base64(dbo.f_hmac_encrypt(convert(varbinary(max), @secret), convert(varbinary(max), @header + '.' + @payload), 'SHA2_256'), 1)


    if @signature_verify != @signature
    begin
        return '{"errorMessage":"Invalid Token", "errorCode":"500"}'
    end


    return convert(varchar(max), dbo.f_base64_to_binary(@payload, 1))
end
GO

-----------------------------
-- JSON Web Token Creation --
-----------------------------
create   function [dbo].[f_jwt_encode](
	@json_header	varchar(max),
	@json_payload	varchar(max),
	@secret			varchar(max)
)
returns varchar(max)
as
begin

	declare @header		varchar(max),
			@data		varchar(max),
			@signature	varchar(max);

	-- Base64 encode json header
	select @header = dbo.f_to_base64(convert(varbinary(max), @json_header), 1);

	-- Base64 encode json payload
	select @data = dbo.f_to_base64(convert(varbinary(max), @json_payload), 1);

	-- Generate signature
	select	@signature = dbo.f_hmac_encrypt(convert(varbinary(max), @secret), convert(varbinary(max), @header + '.' + @data), 'SHA2_256');

	-- Base64 encode signature
	select	@signature = dbo.f_to_base64(convert(varbinary(max), @signature), 1);

	return @header + '.' + @data + '.' + @signature;
end
GO

create function [dbo].[f_to_base64](
  @data varbinary(max),
  @url_safe bit
)
returns varchar(max)
as
begin
  declare @base64string varchar(max)

   -- When converting a table to json, binary data in the table is converted to a BASE64 String
  select @base64string = col
  from openjson(
    (
      select col
      from (select @data col) T
      for json auto
    )
  ) with(col varchar(max))

  if @url_safe = 1
  begin
    select @base64string = replace(@base64string, '+', '-')
    select @base64string = replace(@base64string, '/', '_')
  end

  return @base64string
end
GO

create function [dbo].[f_xml_to_json](@xmldata xml)
returns nvarchar(max)
as
begin

    declare @json nvarchar(max)

    select  @json = concat(@json, ',{'
                    + stuff(
                        (
                            select ',"'
                                    + coalesce(b.c.value('local-name(.)', 'nvarchar(max)'), '')
                                    + '":"'
                                    + b.c.value('text()[1]','nvarchar(max)')
                                    + '"'

                            from    x.a.nodes('*') b(c)

                            for     xml path(''), type
                        ).value('(./text())[1]', 'nvarchar(max)')

                        , 1, 1, ''
                    )
                    + '}')

    from  @xmldata.nodes('/root/*') x(a)

    -- Remove leading comma
    return stuff(@json, 1, 1, '')

end
GO
