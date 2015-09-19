<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:utils="urn:schemas-reflector-com:xslt">
  <xsl:output method="text" indent="yes"/>

  <xsl:template match="/type">
    <xsl:call-template name="renderDefinition"/>
  </xsl:template>

  <xsl:template name="renderDefinition" match="type">
    <xsl:variable name="urlPrefix" select="concat('/', attributes/attribute[@type='System.Web.Http.RoutePrefixAttribute']/properties/property[@name='Prefix']/@value, '/')"/>
    <xsl:variable name="collectionId" select="utils:Guid()"/>
    {
      "id": "<xsl:value-of select="$collectionId"/>",
      "name": "<xsl:value-of select="utils:TypeName(@name)"/>",
      "description": "",
      "order": [
      <xsl:for-each select="methods/method">
        <xsl:call-template name="renderMethodGuid">
          <xsl:with-param name="isLast" select="position() = last()"/>
        </xsl:call-template>
      </xsl:for-each>
      ],
      "folders": [],
      "timestamp": <xsl:value-of select="utils:TimeStamp()"/>,
      "owner": 0,
      "remoteLink": "",
      "public": false,
      "requests": [
      <xsl:for-each select="methods/method">
        <xsl:call-template name="renderRequest">
            <xsl:with-param name="urlPrefix" select="$urlPrefix"/>
            <xsl:with-param name="isLast" select="position() = last()"/>
          <xsl:with-param name="collectionId" select="$collectionId"/>
        </xsl:call-template>          
      </xsl:for-each>
      ]
    }
  </xsl:template>

  <xsl:template name="renderMethodGuid" match="method">
    <xsl:param name="isLast"/>
    "<xsl:value-of select="utils:Guid(@name)"/>"
    <xsl:if test="not($isLast)">
      <xsl:value-of select="string(',')"/>    
    </xsl:if>
  </xsl:template>
  
  <xsl:template name="renderRequest" match="method">
    <xsl:param name="urlPrefix"/>
    <xsl:param name="isLast"/>
    <xsl:param name="collectionId"/>
    
    <xsl:variable name="onBody" select="attributes/attribute/@type='System.Web.Http.FromBodyAttribute'"/>
    {
      "id": "<xsl:value-of select="utils:Guid(@name)"/>",
      <xsl:call-template name="renderHeaders"/>,
      "url": "<xsl:value-of select="concat($urlPrefix, attributes/attribute[@type='System.Web.Http.RouteAttribute']/properties/property[@name='Template']/@value)"/>",
			"preRequestScript": "",
			"pathVariables": {},
			"method": "<xsl:value-of select="utils:HttpMethod(attributes/attribute/@type='System.Web.Http.HttpGetAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPostAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPutAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpPatchAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpDeleteAttribute',
                          attributes/attribute/@type='System.Web.Http.HttpOptionsAttribute')"/>",
			"data": [],
			"dataMode": "<xsl:value-of select="utils:Iif($onBody,'raw','params')"/>",
			"version": 2,
			"tests": "",
			"currentHelper": "normal",
			"helperAttributes": {},
			"time": <xsl:value-of select="utils:TimeStamp()"/>,
			"name": "<xsl:value-of select="utils:CapitalizeWords(@name)"/>",
			"description": "<xsl:value-of select="@name"/>",
			"collectionId": "<xsl:value-of select="$collectionId"/>",
			"responses": []
      <xsl:if test="$onBody">
        ,<xsl:call-template name="renderData"/>
      </xsl:if>
    }
    <xsl:if test="not($isLast)">
      <xsl:value-of select="string(',')"/>
    </xsl:if>
  </xsl:template>
  
  <xsl:template name="renderHeaders">
      "headers": "Content-Type: application/json\nX-Account-Context: {{account}}\nAuthorization: {{auth}}\n"
  </xsl:template>
  
  <xsl:template name="renderData">
    "rawModeData": "
    <xsl:for-each select="parameters/parameter">
      <xsl:call-template name="renderParameter">
      </xsl:call-template>    
    </xsl:for-each>
    "
  </xsl:template>

  <xsl:template name="renderParameter" match="parameter">
      <xsl:if test ="attributes/attribute/@type='System.Web.Http.FromBodyAttribute'">
        <jsonObject>          
          <xsl:value-of select="string('&lt;![CDATA[')" disable-output-escaping="yes"/>
          {<xsl:for-each select="properties/property">
              <xsl:call-template name="renderProperty">
                <xsl:with-param name="isLast" select="position() = last()"/>
              </xsl:call-template>
          </xsl:for-each>
          }
          <xsl:value-of select="string(']]&gt;')" disable-output-escaping="yes"/>
        </jsonObject>
      </xsl:if>
  </xsl:template>

  <xsl:template name="renderProperty" match="property">
    <xsl:param name="isLast"/>

    <xsl:choose>
      <xsl:when test="@isArray = 'true'">
        <xsl:call-template name="renderPropertyAsJsonArray">
          <xsl:with-param name="isLast" select="$isLast"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:when test="properties/property">
        <xsl:call-template name="renderPropertyAsJsonObject">
          <xsl:with-param name="isLast" select="$isLast"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="renderPropertyAsJson">
          <xsl:with-param name="isLast" select="$isLast"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="renderPropertyAsJson" match="property">
    <xsl:param name="isLast"/>
      "<xsl:value-of select="@name"/>": <xsl:choose>
        <xsl:when test="@type = 'System.String'">
          <xsl:value-of select="concat('&quot;',@name, @defaultValue,' {0}&quot;')" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="@defaultValue"/>
        </xsl:otherwise>
      </xsl:choose>
    <xsl:if test="not($isLast)">
      <xsl:value-of select="string(',')"/>
    </xsl:if>
  </xsl:template>

  <xsl:template name="renderPropertyAsJsonArray" match="property">
    <xsl:param name="isLast"/>
        "<xsl:value-of select="@name"/>":
        [<xsl:for-each select="properties/property">
            <xsl:call-template name="renderProperty">
              <xsl:with-param name="isLast" select="position() = last()"/>
            </xsl:call-template>
        </xsl:for-each>
        ]<xsl:if test="not($isLast)"><xsl:value-of select="string(',')"/></xsl:if>
  </xsl:template>

  <xsl:template name="renderPropertyAsJsonObject" match="property">
    <xsl:param name="isLast"/>
    "<xsl:value-of select="@name"/>":
    {
    <xsl:for-each select="properties/property">
      <xsl:call-template name="renderProperty">
        <xsl:with-param name="isLast" select="position() = last()"/>
      </xsl:call-template>
    </xsl:for-each>
    }
    <xsl:if test="not($isLast)">
      <xsl:value-of select="string(',')"/>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
