using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace startup {
	class ProcConfig {
		const string _configPath = @"Config\ProcConfig.xml";
		public IList<IProc> Procs { get; }

		public ProcConfig() {
			Procs = new List<IProc>();
			ReadFromConfig();
		}

		private void ReadFromConfig() {
			var allProcesses = Process.GetProcesses();
			using(var reader = new StreamReader(_configPath)) {
				using(var xmlReader = XmlReader.Create(reader)) {
					while(xmlReader.Read()) {
						if(xmlReader.NodeType == XmlNodeType.Element && xmlReader.HasAttributes) {
							var id = xmlReader.GetAttribute("id");
							if(Procs.Any(p => p.Id == id))
								throw new Exception(string.Format("id already used: {0}", id));
							var path = xmlReader.GetAttribute("path");
							var args = xmlReader.GetAttribute("args");
							var parentProcId = xmlReader.GetAttribute("startAfter");
							var proc = new Proc(id, path, args);
							if(!string.IsNullOrWhiteSpace(parentProcId)) {
								var parentProc = Procs.First(p => p.Id == parentProcId);
								parentProc.ContinueWith(proc);
							} else {
								Procs.Add(proc);
							}
						}
					}
				}

			}
		}
	}
}
