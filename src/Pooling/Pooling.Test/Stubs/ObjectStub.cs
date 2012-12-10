/*********************************************************************************************
 * 
 * Author:
 *      Dmitry Vyakhirev (dmitry.vyakhirev@hotmail.com)
 *
 *********************************************************************************************/

using System;

namespace Pooling.Test.Stubs
{
    /// <summary>
    /// Dummy object stub, which implements <see cref="IDisposable"/>.
    /// </summary>
    public class ObjectStub : IDisposable
    {
        public virtual void Dispose()
        {
        }
    }

}
