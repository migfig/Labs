﻿using System;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Common;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace SpecFlow.Api.Context
{
    public class EndpointContext
    {
        private Table _settings;
        public Table Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
                foreach (var r in _settings.Rows)
                    foreach (var h in _settings.Header)
                    {
                        var key = h.Var();
                        if (!_sessionVars.ContainsKey(key))
                            _sessionVars.Add(key, r[h]);
                    }
            }
        }

        private Dictionary<string, object> _sessionVars = new Dictionary<string, object>();
        public object GetResult(string property)
        {
            return _sessionVars[property.Var()];
        }

        public bool IsTokenValid(string token)
        {
            var tokenVar = token.Var();
            return _sessionVars.ContainsKey(tokenVar) && !string.IsNullOrEmpty(_sessionVars[tokenVar].ToString());
        } 

        public bool Call(Table pars)
        {
            var outputFile = pars.Rows[0].Output();
            if (File.Exists(outputFile)) File.Delete(outputFile);

            var exitCode = Extensions.runProcess(_sessionVars["$program$"].ToString(), pars.Args(_sessionVars));
            if (exitCode != 0) return false;

            var properties = pars.Property().Split(';');
            if (!File.Exists(outputFile)) return false;

            return LoadJsonProps(outputFile, properties);
        }

        private bool LoadJsonProps(string fileName, params string[] property)
        {
            using (var stream = File.OpenText(fileName))
            {
                var isArray = false;
                var isObject = false;
                var code = string.Empty;

                foreach (var p in property)
                {
                    var idx = p.IndexOf('[');
                    var objIdx = p.IndexOf('(');
                    if(idx > 0)
                    {
                        isArray = true;
                        var props = p.Substring(idx + 1).Replace("]", "").Split(',');
                        foreach (var pr in props)
                            code += "public string " + pr + " { get; set; }" + Environment.NewLine;
                    }
                    else if(objIdx > 0)
                    {
                        isObject = true;
                        var props = p.Substring(objIdx + 1).Replace(")", "").Split(',');
                        foreach (var pr in props)
                            code += "public string " + pr + " { get; set; }" + Environment.NewLine;
                    }
                    else 
                        code += "public string " + p + " { get; set; }" + Environment.NewLine;
                }

                code = "using System.Collections.Generic;" + Environment.NewLine + "public class Results {" + code + "}";
                if (isArray)
                    code += Environment.NewLine + "public class ResultsArray: List<Results> {}";
                var classType = BuildRuntimeType(code);

                var json = JsonConvert.DeserializeObject(stream.ReadToEnd(), classType);

                if(json != null)
                {
                    if (!isArray && !isObject)
                    {
                        var type = json.GetType();
                        foreach (var p in property)
                        {
                            var propVar = p.Var();
                            if (!_sessionVars.ContainsKey(propVar))
                                _sessionVars.Add(propVar, type.GetProperty(p).GetValue(json));
                            else
                                _sessionVars[propVar] = type.GetProperty(p).GetValue(json);
                        }
                    }
                    else
                    {
                        var charIdx = isArray ? '[' : '(';
                        var propVar = property.First().Substring(0, property.First().IndexOf(charIdx)).Var();
                        if (!_sessionVars.ContainsKey(propVar))
                            _sessionVars.Add(propVar, json);
                        else
                            _sessionVars[propVar] = json;
                    }
                }
            }

            return true;
        }

        private Type BuildRuntimeType(string code)
        {
            var csProv = new CSharpCodeProvider();
            var result = csProv.CompileAssemblyFromSource(new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                TreatWarningsAsErrors = false                
            }, code);

            if (result.Errors.Count > 0) return null;

            return result.CompiledAssembly.GetTypes().Last();
        }
    }

    public static class ContextExtensions
    {
        public static string Args(this Table pars, Dictionary<string, object> vars)
        {
            var args = string.Format("-X {0} {1} -o {2} {3} {4}",
                pars.Rows[0]["Method"],
                pars.Rows[0]["Url"],
                pars.Rows[0].Output(),
                pars.Rows[0]["Headers"].Headers(),
                pars.Rows[0].Json()).Trim();

            foreach (var key in vars.Keys)
                args = args.Replace(key, vars[key].ToString());

            return args;
        }

        public static string Output(this TableRow row)
        {
            var localPath = string.Join("_", new Uri(row["Url"]).LocalPath.Substring(1).Split('/'));
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                string.Format("{0}_{1}.json", row["Method"], localPath));
        }

        public static string Headers(this string headers)
        {
            var builder = new StringBuilder();
            var hdrs = headers.Split(';').Where(x => !string.IsNullOrEmpty(x));
            foreach (var h in hdrs)
                builder.AppendFormat("-H \"{0}:{1}\" ", h.Split(':')[0].Trim(), h.Split(':')[1].Trim());

            return builder.ToString().Trim();
        }

        public static string Json(this TableRow row)
        {
            var payload = row.Keys.Contains("Payload") ? row["Payload"] : string.Empty;
            return string.IsNullOrEmpty(payload) ? string.Empty : "-d \"" + payload.Replace("\"", "\\\"") + "\"";
        }

        public static string Property(this Table pars)
        {
            return pars.Rows[0]["Property"];
        }

        public static string Var(this string name)
        {
            var names = name.Split('[', '(');
            return "$" + (names.Length == 1 ? name.Trim().ToLower() : names.First()) + "$";
        }
    }
}
