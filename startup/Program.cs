using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace startup {
	class Program {
		static void Main(string[] args) {
			var config = new ProcConfig();
			foreach(var proc in config.Procs) {
				Task.Factory.StartNew(() => proc.Start());
			}
			while(!AllFinished(config.Procs)) {

			}
		}

		static bool AllFinished(IEnumerable<IProc> procs) {
			if(procs.Count() == 0)
				return true;
			var allFinished = procs.All(p => p.IsFinished);
			foreach(var proc in procs) {
				allFinished = allFinished && AllFinished(proc.Children);
			}
			return allFinished;
		}
	}
}
