using System;
using System.Collections.Generic;
using System.IO;
using Tensorflow;

namespace DogsBreedClassification.Classification;

public class DataLoader
{
    #region Singleton
    
    public static DataLoader Instance;

    
    private DataLoader(){}
    
    public static DataLoader getInstance()
    {
        if (Instance == null)
            Instance = new DataLoader();
        return Instance;
    }
    

    #endregion

    public List<ImageData> ImageData = new List<ImageData>();
    
    
    
    public void LoadImages(string path)
    {
        string[] classes = Directory.GetDirectories(path);
        foreach (string s in classes)
        {
            string[] splitted = s.Split('/');
            string clazz = splitted[splitted.Length - 1];

            string[] images = Directory.GetFiles(path + clazz);

            foreach (string image in images)
            {
                var tensor = tf.image.decode_image();
                ImageData.Add(new ImageData()
                {
                    ImagePath = image, Label = clazz
                });
            }
        }
    }

    public void LoadTensors()
    {
        
    }
    
}