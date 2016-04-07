using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace LiveLeakCommentSearch
{
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.String)]
    public class State
    {
        #region Properties
        private bool _forwards;
        private List<string> _targetNames;
        private SearchType _searchType;
        private long _lastId;
        private bool _changed;

        public long Processed { get; set; }
        public string TimeEstimate { get; set; }
        public bool Forwards
        {
            get { return _forwards; }
            set
            {
                if (_forwards == value) return;
                _forwards = value;
                Changed = true;
            }
        }
        public List<string> Names
        {
            get { return _targetNames; }
            set
            {
                if (_targetNames == value) return;
                _targetNames = value;
                Changed = true;
            }
        }
        public SearchType Type
        {
            get { return _searchType; }
            set
            {
                if (_searchType == value) return;
                _searchType = value;
                Changed = true;
            }
        }
        public long StartId { get; set; }
        public long TimeStamp { get; set; }
        public long LastId
        {
            get { return _lastId; }
            set
            {
                if (_lastId == value) return;
                Changed = true;
                _lastId = value;
            }
        }
        public bool Changed
        {
            get { return _changed; }
            set
            {
                _changed = value;
                if (value)
                    OnStateChanged(EventArgs.Empty);
            }
        }
        #endregion

        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public event ChangedEventHandler StateChanged;

        public State()
        {
            Changed = false;
        }
        public void Update(long lastId, SearchType type, List<string> targetNames
                            ,bool direction, string time, bool increment = true)
        {
            _lastId = lastId;
            _searchType = type;
            _targetNames = targetNames;
            _forwards = direction;
            if (increment) Processed++;
            Changed = true;
            TimeEstimate = time;
            TimeStamp = DateTime.Now.Ticks;
        }
        public string Serialise()
        {
            var xmlSerializer = new XmlSerializer(typeof (State));
            var sw = new StringWriter();
            xmlSerializer.Serialize(sw, this);
            return sw.ToString();
        }
        public void Deserialise(string stateObj)
        {
            var xmlSerializer = new XmlSerializer(typeof (State));
            var sr = new StringReader(stateObj);
            XmlReader xr = new XmlTextReader(sr);
            State newState;
            try
            {
                newState = (State)xmlSerializer.Deserialize(xr);
            }
            catch (InvalidOperationException)
            {
                newState = new State();
            }

            Forwards    = newState.Forwards;
            LastId      = newState.LastId;
            Names       = newState.Names;
            Processed   = newState.Processed;
            StartId     = newState.StartId;
            TimeEstimate = newState.TimeEstimate;
            TimeStamp   = newState.TimeStamp;
            Type        = newState.Type;
        }
        public void RestoreStateFromFile(string filename)
        {
            Deserialise(File.ReadAllText(filename));
        }
        public virtual void OnStateChanged(EventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }
    }
}