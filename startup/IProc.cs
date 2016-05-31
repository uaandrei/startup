using System.Collections.Generic;

namespace startup {
	internal interface IProc {
		string Id { get; }
		IEnumerable<IProc> Children { get; }
		bool IsFinished { get; }
		void Start();
		IProc ContinueWith(IProc p);
	}
}