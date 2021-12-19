using Microsoft.ML;
using Microsoft.ML.Data;

namespace SentimentAnalysis
{
  class Program
  {
    static readonly string _path = Path.Combine(Environment.CurrentDirectory,"Data", "yelp_labelled.txt");

    static readonly string[] _samples =
    {
            "Мы не вернемся назад.",
            "Ужасное место",
            "Очень вежливый персонал",
            "Еда была очень вкусной",
            "Я бы свою собаку сюда не привёл"
        };

    static void Main(string[] args)
    {
      var context = new MLContext(seed: 0);

      var data = context.Data.LoadFromTextFile<Input>(_path, hasHeader: false);
      var trainTestData = context.Data.TrainTestSplit(data, testFraction: 0.2, seed: 0);
      var trainData = trainTestData.TrainSet;
      var testData = trainTestData.TestSet;
      var pipeline = context.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: "SentimentText")
          .Append(context.BinaryClassification.Trainers.FastTree(numberOfLeaves: 50, minimumExampleCountPerLeaf: 20));

      Console.WriteLine("Тренирую модель...");
      var model = pipeline.Fit(trainData);


      var predictions = model.Transform(testData);
      var metrics = context.BinaryClassification.Evaluate(predictions, "Label");

      Console.WriteLine();
      Console.WriteLine($"Точность: {metrics.Accuracy:P2}");
      Console.WriteLine($"AUC: {metrics.AreaUnderPrecisionRecallCurve:P2}");
      Console.WriteLine($"F1: {metrics.F1Score:P2}");
      Console.WriteLine();




      var scores = context.BinaryClassification.CrossValidate(data, pipeline, numberOfFolds: 5);
      var mean = scores.Average(x => x.Metrics.F1Score);
      Console.WriteLine($"Средний балл F1 с перекрестной проверкой:{mean:P2}");

  
      var predictor = context.Model.CreatePredictionEngine<Input, Output>(model);
      foreach (var sample in _samples)
      {
        var input = new Input { SentimentText = sample };
        var prediction = predictor.Predict(input);

        Console.WriteLine();
        Console.WriteLine($"{input.SentimentText}");
        Console.WriteLine($"Оценка: {prediction.Probability}");
        Console.WriteLine($"{(Convert.ToBoolean(prediction.Prediction) ? "Позитивный" : "Негативный")} отзыв");
      }

      Console.WriteLine();
    }
  }



  public class Input
  {
    [LoadColumn(0)]
    public string SentimentText;

    [LoadColumn(1), ColumnName("Label")]
    public bool Sentiment;
  }


  public class Output
  {
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }
    public float Probability { get; set; }
  }
}