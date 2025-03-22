using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace fwp.gamepad.state
{
    [System.Serializable]
    public class ControllerState
    {
        public InputType inputCateogry;

        public ControllerState(InputType cat)
        {
            inputCateogry = cat;
        }

        /// <summary>
        /// Perform a deep copy of the object via serialization.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>A deep copy of the object.</returns>
        public ControllerState Clone()
        {
            if (!typeof(ControllerState).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.");
            }

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(this, null)) return default;

            using var stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            return (ControllerState)formatter.Deserialize(stream);
        }
    }

}