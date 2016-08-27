<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes" standalone="yes" cdata-section-elements="Code"/>

  <xsl:template match="tokens">
    <Slide>
      <xsl:attribute name="Title">
        <xsl:value-of select="token[@type='H1'][@value]"/>
      </xsl:attribute>
      <RichTextBlock FontSize="20" FontWeight="DemiBold" LineHeight="40">
        <Paragraph>
          <xsl:apply-templates select="token"/>
        </Paragraph>
      </RichTextBlock>
      <xsl:apply-templates select="code"/>
    </Slide>
  </xsl:template>

  <xsl:template match="token[@type='Bold']">
      <Bold>
        <xsl:value-of select="@value"/>
      </Bold>
  </xsl:template>

  <xsl:template match="token[@type='Italics']">
      <Run>
        <xsl:attribute name="FontStyle">
          <xsl:value-of select="string('Italic')"/>
        </xsl:attribute>
        <xsl:value-of select="@value"/>
      </Run>
  </xsl:template>

  <xsl:template match="token[@type='Regular']">
    <Run>
      <xsl:value-of select="@value"/>
    </Run>
  </xsl:template>

  <xsl:template match="token[@type='Strikethrough']">
    <Run>
      <xsl:value-of select="@value"/>
    </Run>
  </xsl:template>

  <xsl:template match="token[@type='Blockquotes']">
    <Run>
      <xsl:value-of select="@value"/>
    </Run>
  </xsl:template>

  <xsl:template match="token[@type='Indented']">
    <Run TextIndent="4">
      <xsl:value-of select="@value"/>
    </Run>
  </xsl:template>

  <xsl:template match="token[@type='NewLine']">
      <LineBreak />
  </xsl:template>

  <xsl:template match="code">
    <Component>
      <xsl:attribute name="Language">
        <xsl:value-of select="@language"/>
      </xsl:attribute>
      <Code>
        <xsl:value-of select="text()"/>
      </Code>
    </Component>
  </xsl:template>
</xsl:stylesheet>
