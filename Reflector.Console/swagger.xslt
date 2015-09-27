<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:utils="urn:schemas-reflector-com:xslt">
  <xsl:output method="text" indent="yes"/>

  <xsl:template match="/type">
    <xsl:call-template name="renderSwagger"/>
  </xsl:template>

  <xsl:template name="renderSwagger" match="type">
    <xsl:variable name="urlPrefix" select="concat('/', attributes/attribute[@type='System.Web.Http.RoutePrefixAttribute']/properties/property[@name='Prefix']/@value, '/')"/>
    swagger: "2.0"
    info:
    version: "0.0.1"
    title: <xsl:value-of select="utils:CapitalizeWords(utils:CompleteTypeName(@name))"/>
    # during dev, should point to your local machine
    host: localhost:60264
    # basePath prefixes all resource paths
    basePath: <xsl:value-of select="$urlPrefix"/>
    #
    schemes:
    # tip: remove http to make production-grade
    - http
    - https
    # format of bodies a client can send (Content-Type)
    consumes:
    - application/json
    # format of the responses to the client (Accepts)
    produces:
    - application/json
    paths:
    <xsl:for-each select="methods/method">
      <xsl:call-template name="renderPath">
        <xsl:with-param name="urlPrefix" select="$urlPrefix"/>
      </xsl:call-template>
    </xsl:for-each>

    # complex objects have schema definitions
    definitions:
    <xsl:for-each select="methods/method">
      <xsl:call-template name="renderSchemaDefinition">
        <xsl:with-param name="urlPrefix" select="$urlPrefix"/>
      </xsl:call-template>
    </xsl:for-each>

    ErrorResponse:
    required:
    - message
    properties:
    message:
    type: string
  </xsl:template>

  <xsl:template name="renderPath" match="method">
    <xsl:param name="urlPrefix"/>
    
    <xsl:value-of select="concat($urlPrefix, attributes/attribute[@type='System.Web.Http.RouteAttribute']/properties/property[@name='Template']/@value)"/>:
    # binds app logic to a route
    x-swagger-router-controller: <xsl:value-of select="@name"/>
    <xsl:value-of select="utils:NewLine()"/> 
    <xsl:value-of select="utils:ToLower(utils:HttpMethod(attributes/attribute/@type='System.Web.Http.HttpGetAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPostAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPutAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPatchAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpDeleteAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpOptionsAttribute'))"/>:
    description: <xsl:value-of select="utils:CapitalizeWords(@name)"/>
    # used as the method name of the controller
    operationId: <xsl:value-of select="@name"/>
    parameters:
    <xsl:for-each select="parameters/parameter">
      <xsl:call-template name="renderParameter"/>
    </xsl:for-each>
    responses:
    "200":
    description: Success
    schema:
    # a pointer to a definition
    $ref: "#/definitions/<xsl:value-of select="utils:TypeName(utils:FixType(@type))"/>"
    # responses may fall through to errors
    default:
    description: Error
    schema:
    $ref: "#/definitions/ErrorResponse"
  </xsl:template>

  <xsl:template name="renderParameter" match="parameter">
    - name: <xsl:value-of select="@name"/>
    in: <xsl:value-of select="utils:Iif(attributes/attribute/@type='System.Web.Http.FromBodyAttribute', 'body', 'query')"/>
    description: <xsl:value-of select="@name"/> description
    required: true
    type: <xsl:value-of select="@type"/>
  </xsl:template>

  <xsl:template name="renderSchemaDefinition" match="method">
    <xsl:param name="urlPrefix"/>
    <xsl:choose>
      <xsl:when test="@itemType">
        <xsl:value-of select="concat(@itemType,'[]')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="attributes/attribute[@type='System.Web.Http.Description.ResponseTypeAttribute']/properties/property[@name='ResponseType']/@value"/>
      </xsl:otherwise>
    </xsl:choose>:
    required:
    <xsl:for-each select="properties/property">
      - <xsl:value-of select="@name"/>
    </xsl:for-each>
    properties:
    <xsl:for-each select="properties/property">
      <xsl:call-template name="renderProperty"/>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="renderProperty" match="property">
    <xsl:value-of select="@name"/>:
    type: <xsl:value-of select="@type"/>
  </xsl:template>
</xsl:stylesheet>
