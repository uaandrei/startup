using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace startup {
	class Proc : IProc {
		private Process _process;
		private List<IProc> _procs = new List<IProc>();
		public string Id { get; private set; }
		public bool IsFinished { get; private set; }
		public IEnumerable<IProc> Children {
			get {
				return _procs;
			}
		}

		public Proc(string id, string path, string args) {
			Id = id;
			_process = new Process();
			_process.StartInfo = new ProcessStartInfo(path);
			if(!string.IsNullOrWhiteSpace(args))
				_process.StartInfo.Arguments = args;
		}

		public IProc ContinueWith(IProc p) {
			if(_procs.Any(proc => proc.Id == p.Id))
				throw new Exception(string.Format("child proc alredy exists: {0}", p.Id));
			_procs.Add(p);
			return p;
		}

		public void Start() {
			_process.Start();
			if(_procs.Count > 0) {
				_process.WaitForExit();
				_procs.ForEach(p => p.Start());
			}
			IsFinished = true;
		}

		public override string ToString() {
			return Id;
		}
	}
}
