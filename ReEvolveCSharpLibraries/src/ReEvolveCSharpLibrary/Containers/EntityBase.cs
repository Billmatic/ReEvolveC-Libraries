using System;

namespace ReEvolveCSharpLibrary.Containers
{
    /// <summary>
	/// Base entity class.
	/// </summary>
    [Serializable]
    public class EntityBase : ICloneable
    {
        #region Properties.

        /// <summary>
        /// Gets or sets the name of the user that created the entity.
        /// </summary>
        /// <value>Created by user.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date/time the entity was created.
        /// </summary>
        /// <value>Created on date/time.</value>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active.
        /// </summary>
        /// <value>true if the entity is active; otherwise false.</value>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that last modified the entity.
        /// </summary>
        /// <value>Modified by user.</value>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date/time the entity was last modified.
        /// </summary>
        /// <value>Modified on date/time.</value>
        public DateTime? ModifiedOn { get; set; }

        #endregion

        #region Clone method.

        /// <summary>
        /// Clone the entity.
        /// </summary>
        /// <returns>The cloned entity.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
