using System.Diagnostics;

namespace RocksIsolationPrototype;

public interface INeedsToBeMeaningOfEverything
{
    int Value { get; }
}

public enum GlassState
{
    HalfFull,
    HalfEmpty
}

public class PessimisticComponent
{
    public virtual GlassState GetState() => GlassState.HalfEmpty;
}

public class MockMeIfYouCan
{
    public int Unmockable() => -42;
}

public interface IIrrelevant0
{
    void Void();
}

public interface IIrrelevant1
{
    int HamsterTreadmillRounds { get; }
}

public class Subject
{
    private readonly PessimisticComponent _pessimisticComponent;
    public required INeedsToBeMeaningOfEverything MeaningOfEverything { get; init; }
    public required IIrrelevant0 Irrelevant0 { get; init; }
    
    public Subject(PessimisticComponent pessimisticComponent, IIrrelevant1 irrelevant1, MockMeIfYouCan mockMeIfYouCan)
    {
        _pessimisticComponent = pessimisticComponent;
    }

    public bool ToBeTested() => _pessimisticComponent.GetState() == GlassState.HalfFull && MeaningOfEverything.Value == 42;
}

public static class Test
{
    public static void Run()
    {
        /* This is the code necessary for expectation management without isolation
        var meaningExpectations = Rock.Create<INeedsToBeMeaningOfEverything>();
        meaningExpectations.Properties().Getters().Value().Returns(42);
        var pessimisticComponentExpectations = Rock.Create<PessimisticComponent>();
        pessimisticComponentExpectations.Methods().GetState().Returns(GlassState.HalfFull);
        var irrelevant0Make = Rock.Make<IIrrelevant0>();
        var irrelevant1Make = Rock.Make<IIrrelevant1>();

        var subject = new Subject(pessimisticComponentExpectations.Instance(), irrelevant1Make.Instance(), new MockMeIfYouCan())
        {
            MeaningOfEverything = meaningExpectations.Instance(),
            Irrelevant0 = irrelevant0Make.Instance()
        };
        */

        var subjectIsolation = RockProto.Isolate<Subject>();
        subjectIsolation.Properties().MeaningOfEverything.Properties().Getters().Value().Returns(42);
        subjectIsolation.ConstructorParameters().PessimisticComponent.Methods().GetState().Returns(GlassState.HalfFull);
        
        var subject = subjectIsolation.Instance(new MockMeIfYouCan());

        var result = subject.ToBeTested();

        Debug.Assert(result);
        
        subjectIsolation.Verify();
        
        /* Also the verification without isolation
        meaningExpectations.Verify();
        pessimisticComponentExpectations.Verify();
        */
    }
}