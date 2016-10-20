﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Microsoft.Build.Framework
{
    /// <summary>
    /// Arguments for project started events
    /// </summary>
    /// <remarks>
    /// WARNING: marking a type [Serializable] without implementing
    /// ISerializable imposes a serialization contract -- it is a
    /// promise to never change the type's fields i.e. the type is
    /// immutable; adding new fields in the next version of the type
    /// without following certain special FX guidelines, can break both
    /// forward and backward compatibility
    /// </remarks>
    [Serializable]
    public class ProjectStartedEventArgs : BuildStatusEventArgs
    {
        #region Constants
        public const int InvalidProjectId = -1;
        #endregion

        /// <summary>
        /// Default constructor 
        /// </summary>
        protected ProjectStartedEventArgs()
            : base()
        {
            // do nothing
        }

        /// <summary>
        /// This constructor allows event data to be initialized.
        /// Sender is assumed to be "MSBuild".
        /// </summary>
        /// <param name="message">text message</param>
        /// <param name="helpKeyword">help keyword </param>
        /// <param name="projectFile">project name</param>
        /// <param name="targetNames">targets we are going to build (empty indicates default targets)</param>
        /// <param name="properties">list of properties</param>
        /// <param name="items">list of items</param>
        public ProjectStartedEventArgs
        (
            string message,
            string helpKeyword,
            string projectFile,
            string targetNames,
            IEnumerable properties,
            IEnumerable items
        )
            : this(message, helpKeyword, projectFile, targetNames, properties, items, DateTime.UtcNow)
        {
        }

        /// <summary>
        /// This constructor allows event data to be initialized.
        /// Sender is assumed to be "MSBuild".
        /// </summary>
        /// <param name="projectId">project id</param>
        /// <param name="message">text message</param>
        /// <param name="helpKeyword">help keyword </param>
        /// <param name="projectFile">project name</param>
        /// <param name="targetNames">targets we are going to build (empty indicates default targets)</param>
        /// <param name="properties">list of properties</param>
        /// <param name="items">list of items</param>
        /// <param name="parentBuildEventContext">event context info for the parent project</param>
        public ProjectStartedEventArgs
        (
            int projectId,
            string message,
            string helpKeyword,
            string projectFile,
            string targetNames,
            IEnumerable properties,
            IEnumerable items,
            BuildEventContext parentBuildEventContext
        )
            : this(projectId, message, helpKeyword, projectFile, targetNames, properties, items, parentBuildEventContext, DateTime.UtcNow)
        {
        }

        /// <summary>
        /// This constructor allows event data to be initialized.
        /// Sender is assumed to be "MSBuild".
        /// </summary>
        /// <param name="projectId">project id</param>
        /// <param name="message">text message</param>
        /// <param name="helpKeyword">help keyword </param>
        /// <param name="projectFile">project name</param>
        /// <param name="targetNames">targets we are going to build (empty indicates default targets)</param>
        /// <param name="properties">list of properties</param>
        /// <param name="items">list of items</param>
        /// <param name="parentBuildEventContext">event context info for the parent project</param>
        public ProjectStartedEventArgs
        (
            int projectId,
            string message,
            string helpKeyword,
            string projectFile,
            string targetNames,
            IEnumerable properties,
            IEnumerable items,
            BuildEventContext parentBuildEventContext,
            IDictionary<string, string> globalProperties,
            string toolsVersion
        )
            : this(projectId, message, helpKeyword, projectFile, targetNames, properties, items, parentBuildEventContext)
        {
            this.GlobalProperties = globalProperties;
            this.ToolsVersion = toolsVersion;
        }

        /// <summary>
        /// This constructor allows event data to be initialized. Also the timestamp can be set
        /// Sender is assumed to be "MSBuild".
        /// </summary>
        /// <param name="message">text message</param>
        /// <param name="helpKeyword">help keyword </param>
        /// <param name="projectFile">project name</param>
        /// <param name="targetNames">targets we are going to build (empty indicates default targets)</param>
        /// <param name="properties">list of properties</param>
        /// <param name="items">list of items</param>
        public ProjectStartedEventArgs
        (
            string message,
            string helpKeyword,
            string projectFile,
            string targetNames,
            IEnumerable properties,
            IEnumerable items,
            DateTime eventTimestamp
        )
            : base(message, helpKeyword, "MSBuild", eventTimestamp)
        {
            this.projectFile = projectFile;

            if (targetNames == null)
            {
                this.targetNames = String.Empty;
            }
            else
            {
                this.targetNames = targetNames;
            }

            this.properties = properties;
            this.items = items;
        }

        /// <summary>
        /// This constructor allows event data to be initialized.
        /// Sender is assumed to be "MSBuild".
        /// </summary>
        /// <param name="projectId">project id</param>
        /// <param name="message">text message</param>
        /// <param name="helpKeyword">help keyword </param>
        /// <param name="projectFile">project name</param>
        /// <param name="targetNames">targets we are going to build (empty indicates default targets)</param>
        /// <param name="properties">list of properties</param>
        /// <param name="items">list of items</param>
        /// <param name="parentBuildEventContext">event context info for the parent project</param>
        public ProjectStartedEventArgs
        (
            int projectId,
            string message,
            string helpKeyword,
            string projectFile,
            string targetNames,
            IEnumerable properties,
            IEnumerable items,
            BuildEventContext parentBuildEventContext,
            DateTime eventTimestamp
        )
            : this(message, helpKeyword, projectFile, targetNames, properties, items, eventTimestamp)
        {
            parentProjectBuildEventContext = parentBuildEventContext;
            this.projectId = projectId;
        }

        // ProjectId is only contained in the project started event.
        // This number indicated the instance id of the project and can be
        // used when debugging to determine if two projects with the same name
        // are the same project instance or different instances
        [OptionalField(VersionAdded = 2)]
        private int projectId;

        public int ProjectId
        {
            get
            {
                return projectId;
            }
        }

        [OptionalField(VersionAdded = 2)]
        private BuildEventContext parentProjectBuildEventContext;

        /// <summary>
        /// Event context information, where the event was fired from in terms of the build location
        /// </summary>
        public BuildEventContext ParentProjectBuildEventContext
        {
            get
            {
                return parentProjectBuildEventContext;
            }
        }

        /// <summary>
        /// The name of the project file
        /// </summary>
        private string projectFile;

        /// <summary>
        /// Project name
        /// </summary>
        public string ProjectFile
        {
            get
            {
                return projectFile;
            }
        }

        /// <summary>
        /// Targets that we will build in the project
        /// </summary>
        private string targetNames;

        /// <summary>
        /// Targets that we will build in the project
        /// </summary>
        public string TargetNames
        {
            get
            {
                return targetNames;
            }
        }

        /// <summary>
        /// Gets the set of global properties used to evaluate this project.
        /// </summary>
        [OptionalField(VersionAdded = 2)]
        private IDictionary<string, string> globalProperties;

        /// <summary>
        /// Gets the set of global properties used to evaluate this project.
        /// </summary>
        public IDictionary<string, string> GlobalProperties
        {
            get
            {
                return globalProperties;
            }

            internal set
            {
                globalProperties = value;
            }
        }

        [OptionalField(VersionAdded = 2)]
        private string toolsVersion;

        /// <summary>
        /// Gets the tools version used to evaluate this project.
        /// </summary>
        public string ToolsVersion
        {
            get
            {
                return toolsVersion;
            }

            internal set
            {
                toolsVersion = value;
            }
        }

        // IEnumerable is not a serializable type. That is okay because
        // (a) this event will not be thrown by tasks, so it should not generally cross AppDomain boundaries
        // (b) this event still makes sense when this field is "null"
        [NonSerialized]
        private IEnumerable properties;

        /// <summary>
        /// List of properties in this project. This is a live, read-only list.
        /// </summary>
        public IEnumerable Properties
        {
            get
            {
                // UNDONE: (Serialization.) Rather than storing the properties directly in this class, we could
                // grab them from the BuildRequestConfiguration associated with this projectId (which is now poorly
                // named because it is actually the BuildRequestConfigurationId.)  For central loggers in the
                // multi-proc case, this could pull up just the global properties used to start the project.  For
                // distributed loggers in the multi-proc case and all loggers in the single-proc case, this could pull
                // up the live list of properties from the loaded project, which is stored in the configuration as well.
                // By doing this, we no longer need to transmit properties using this message because they've already
                // been transmitted as part of the BuildRequestConfiguration.
                return properties;
            }
        }

        // IEnumerable is not a serializable type. That is okay because
        // (a) this event will not be thrown by tasks, so it should not generally cross AppDomain boundaries
        // (b) this event still makes sense when this field is "null"
        [NonSerialized]
        private IEnumerable items;

        /// <summary>
        /// List of items in this project. This is a live, read-only list.
        /// </summary>
        public IEnumerable Items
        {
            get
            {
                // UNDONE: (Serialization.) Currently there is a "bug" in the old OM in that items are not transported to
                // the central logger in the multi-proc case.  No one uses this though, so it's probably no big deal.  In
                // the new OM, this list of items could come directly from the BuildRequestConfiguration, which has access
                // to the loaded project.  For distributed loggers in the multi-proc case and all loggers in the single-proc
                // case, this access is to the live list.  For the central logger in the multi-proc case, the main node 
                // has likely not loaded this project, and therefore the live items would not be available to them, which is 
                // the same as the current functionality.
                return items;
            }
        }

        #region CustomSerializationToStream

        /// <summary>
        /// Serializes to a stream through a binary writer
        /// </summary>
        /// <param name="writer">Binary writer which is attached to the stream the event will be serialized into</param>
        internal override void WriteToStream(BinaryWriter writer)
        {
            base.WriteToStream(writer);
            writer.Write((Int32)projectId);
            #region ParentProjectBuildEventContext
            if (parentProjectBuildEventContext == null)
            {
                writer.Write((byte)0);
            }
            else
            {
                writer.Write((byte)1);
                writer.Write((Int32)parentProjectBuildEventContext.NodeId);
                writer.Write((Int32)parentProjectBuildEventContext.ProjectContextId);
                writer.Write((Int32)parentProjectBuildEventContext.TargetId);
                writer.Write((Int32)parentProjectBuildEventContext.TaskId);
                writer.Write((Int32)parentProjectBuildEventContext.SubmissionId);
                writer.Write((Int32)parentProjectBuildEventContext.ProjectInstanceId);
            }
            #endregion
            #region ProjectFile
            if (projectFile == null)
            {
                writer.Write((byte)0);
            }
            else
            {
                writer.Write((byte)1);
                writer.Write(projectFile);
            }
            #endregion

            #region TargetNames
            // TargetNames cannot be null as per line 61 in the constructor
            writer.Write(targetNames);
            #endregion

            #region Properties

            Dictionary<string, string> propertyList = GeneratePropertyList();

            // If no properties were added to the property list 
            // then we have nothing to create when it is deserialized
            // This can happen if properties is null or if none of the 
            // five properties were found in the property object.
            if ((propertyList == null || propertyList.Count == 0))
            {
                writer.Write((byte)0);
            }
            else
            {
                writer.Write((byte)1);

                // Write how many properties we are going to write into the stream
                writer.Write((Int32)propertyList.Count);

                // Write the actual property name value pairs into the stream
                foreach (KeyValuePair<string, string> propertyPair in propertyList)
                {
                    writer.Write(propertyPair.Key);
                    writer.Write(propertyPair.Value);
                }
            }

            #endregion
        }

        /// <summary>
        /// Generates a list of KeyValuePairs from the properties enumerator.
        /// For each of these properties add them to a list to return to the caller.
        /// </summary>
        /// <returns>Null if properties is null, or a list containing one or more of the  properties in the properties enumerator</returns>
        private Dictionary<string, string> GeneratePropertyList()
        {
            if (properties == null)
            {
                return null;
            }

            Dictionary<string, string> propertyList = new Dictionary<string, string>();

            // Loop through the properties and add them to the keyvalue pair list
            foreach (DictionaryEntry property in properties)
            {
                object propertyKey = property.Key;
                object propertyValue = property.Value;

                // Make sure property keys and values are not null before casting.
                // property key and value will always be a string, if this is not the case
                // the a cast exception is the correct course of action. 
                if (property.Key != null && property.Value != null)
                {
                    propertyList.Add((string)property.Key, (string)property.Value);
                }
            }
            return propertyList;
        }

        /// <summary>
        /// Deserializes from a stream through a binary reader
        /// </summary>
        /// <param name="reader">Binary reader which is attached to the stream the event will be deserialized from</param>
        /// <param name="version">The version of the runtime the message packet was created from</param>
        internal override void CreateFromStream(BinaryReader reader, int version)
        {
            base.CreateFromStream(reader, version);
            projectId = reader.ReadInt32();
            #region ParentProjectBuildEventContext
            if (reader.ReadByte() == 0)
            {
                parentProjectBuildEventContext = null;
            }
            else
            {
                int nodeId = reader.ReadInt32();
                int projectContextId = reader.ReadInt32();
                int targetId = reader.ReadInt32();
                int taskId = reader.ReadInt32();

                if (version > 20)
                {
                    int submissionId = reader.ReadInt32();
                    int projectInstanceId = reader.ReadInt32();
                    parentProjectBuildEventContext = new BuildEventContext(submissionId, nodeId, projectInstanceId, projectContextId, targetId, taskId);
                }
                else
                {
                    parentProjectBuildEventContext = new BuildEventContext(nodeId, targetId, projectContextId, taskId);
                }
            }
            #endregion
            #region ProjectFile
            if (reader.ReadByte() == 0)
            {
                projectFile = null;
            }
            else
            {
                projectFile = reader.ReadString();
            }
            #endregion
            #region TargetNames
            // TargetNames cannot be null as per line 61 in the constructor
            targetNames = reader.ReadString();
            #endregion
            #region Properties

            // Check to see if properties was null
            if (reader.ReadByte() == 0)
            {
                properties = null;
            }
            else
            {
                // Get number of properties put on the stream
                int numberOfProperties = reader.ReadInt32();

                // We need to use a dictionaryEntry as that is what the old behavior was
                ArrayList dictionaryList = new ArrayList(numberOfProperties);

                // Read off each of the key value pairs and put them into the dictionaryList
                for (int i = 0; i < numberOfProperties; i++)
                {
                    string key = reader.ReadString();
                    string value = reader.ReadString();

                    if (key != null && value != null)
                    {
                        DictionaryEntry entry = new DictionaryEntry(key, value);
                        dictionaryList.Add(entry);
                    }
                }

                properties = dictionaryList;
            }

            #endregion
        }
        #endregion

        #region SerializationSection
        [OnDeserializing] // Will happen before the object is deserialized
        private void SetDefaultsBeforeSerialization(StreamingContext sc)
        {
            projectId = InvalidProjectId;
            // Don't want to set the default before deserialization is completed to a new event context because
            // that would most likely be a lot of wasted allocations
            parentProjectBuildEventContext = null;
        }

        [OnDeserialized]
        private void SetDefaultsAfterSerialization(StreamingContext sc)
        {
            if (parentProjectBuildEventContext == null)
            {
                parentProjectBuildEventContext = new BuildEventContext
                                                (
                                                    BuildEventContext.InvalidNodeId,
                                                    BuildEventContext.InvalidTargetId,
                                                    BuildEventContext.InvalidProjectContextId,
                                                    BuildEventContext.InvalidTaskId
                                                 );
            }
        }
        #endregion
    }
}
