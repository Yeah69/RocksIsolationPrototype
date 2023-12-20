using Rocks;
using Rocks.Expectations;

namespace RocksIsolationPrototype;

public static class RockProto 
{
    public static SubjectIsolation Isolate<T>() where T : class
    {
        if (typeof(T) == typeof(Subject))
        {
            return new SubjectIsolation();
        }
        throw new Exception("This should never happen");
    }
}

public class SubjectIsolation
{
    private readonly Lazy<IsoProperties> _properties;
    private readonly Lazy<IsoConstructorParameters> _constructorParameters;
    
    private readonly Lazy<Expectations<INeedsToBeMeaningOfEverything>> _prop_meaningOfEverything = new(() => Rock.Create<INeedsToBeMeaningOfEverything>());
    private readonly Lazy<Expectations<IIrrelevant0>> _prop_irrelevant0 = new(() => Rock.Create<IIrrelevant0>());
    private readonly Lazy<Expectations<PessimisticComponent>> _constr_pessimisticComponent = new(() => Rock.Create<PessimisticComponent>());
    private readonly Lazy<Expectations<IIrrelevant1>> _constr_irrelevant1 = new(() => Rock.Create<IIrrelevant1>());
    
    public SubjectIsolation()
    {
        _properties = new Lazy<IsoProperties>(() => new IsoProperties(this));
        _constructorParameters = new Lazy<IsoConstructorParameters>(() => new IsoConstructorParameters(this));
    }
    
    public IsoProperties Properties() => _properties.Value;
    public IsoConstructorParameters ConstructorParameters() => _constructorParameters.Value;
    
    public Subject Instance(MockMeIfYouCan mockMeIfYouCan) => 
        new(_constr_pessimisticComponent.IsValueCreated ? _constr_pessimisticComponent.Value.Instance() : Rock.Make<PessimisticComponent>().Instance(), 
            _constr_irrelevant1.IsValueCreated ? _constr_irrelevant1.Value.Instance() : Rock.Make<IIrrelevant1>().Instance(),
            mockMeIfYouCan)
        {
            MeaningOfEverything = _prop_meaningOfEverything.IsValueCreated ? _prop_meaningOfEverything.Value.Instance() : Rock.Make<INeedsToBeMeaningOfEverything>().Instance(),
            Irrelevant0 = _prop_irrelevant0.IsValueCreated ? _prop_irrelevant0.Value.Instance() : Rock.Make<IIrrelevant0>().Instance()
        };
    
    public void Verify()
    {
        if (_prop_meaningOfEverything.IsValueCreated)
        {
            _prop_meaningOfEverything.Value.Verify();
        }
        if (_prop_irrelevant0.IsValueCreated)
        {
            _prop_irrelevant0.Value.Verify();
        }
        if (_constr_pessimisticComponent.IsValueCreated)
        {
            _constr_pessimisticComponent.Value.Verify();
        }
        if (_constr_irrelevant1.IsValueCreated)
        {
            _constr_irrelevant1.Value.Verify();
        }
    }

    public class IsoProperties
    {
        private readonly SubjectIsolation _subjectIsolation;
        public Expectations<INeedsToBeMeaningOfEverything> MeaningOfEverything => _subjectIsolation._prop_meaningOfEverything.Value;
        public Expectations<IIrrelevant0> Irrelevant0 => _subjectIsolation._prop_irrelevant0.Value;
        
        public IsoProperties(SubjectIsolation subjectIsolation)
        {
            _subjectIsolation = subjectIsolation;
        }
    }
    
    public class IsoConstructorParameters
    {
        private readonly SubjectIsolation _subjectIsolation;
        public Expectations<PessimisticComponent> PessimisticComponent => _subjectIsolation._constr_pessimisticComponent.Value;
        public Expectations<IIrrelevant1> Irrelevant1 => _subjectIsolation._constr_irrelevant1.Value;
        
        public IsoConstructorParameters(SubjectIsolation subjectIsolation)
        {
            _subjectIsolation = subjectIsolation;
        }
    }
}