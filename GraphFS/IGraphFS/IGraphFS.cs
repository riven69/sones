﻿using System;
using System.Collections.Generic;
using sones.PropertyHyperGraph;

namespace sones.GraphFS
{
    /// <summary>
    /// The interface for all kinds of GraphFS
    /// </summary>
    public interface IGraphFS
    {
        #region Information Methods

        #region IsPersistent

        Boolean IsPersistent { get; }

        #endregion

        #region isMounted

        /// <summary>
        /// Returns true if the file system was mounted correctly
        /// </summary>
        /// <returns>true if the file system was mounted correctly</returns>
        Boolean IsMounted { get; }

        #endregion

        #region HasRevisions

        /// <summary>
        /// Determines whether this filesystem uses revisions
        /// </summary>
        Boolean HasRevisions { get; }

        #endregion

        #region HasEditions

        /// <summary>
        /// Determines whether this filesystem uses editions
        /// </summary>
        Boolean HasEditions { get; }

        #endregion

        #region GetFileSystemDescription(...)

        /// <summary>
        /// Returns the name or a description of this file system.
        /// </summary>
        /// <returns>The name or a description of this file system</returns>
        String GetFileSystemDescription();

        #endregion

        #region GetNumberOfBytes(...)

        /// <summary>
        /// Returns the size (number of bytes) of this file system
        /// </summary>
        /// <returns>The size (number of bytes) of this file system</returns>
        UInt64 GetNumberOfBytes();

        #endregion

        #region GetNumberOfFreeBytes(...)

        /// <summary>
        /// Returns the number of free bytes of this file system
        /// </summary>
        /// <returns>The number of free bytes of this file system</returns>
        UInt64 GetNumberOfFreeBytes();

        #endregion

        #region GetAccessMode(...)

        /// <summary>
        /// Returns the access mode of this file system
        /// </summary>
        /// <returns>The access mode of this file system</returns>
        FileSystemAccessMode GetAccessMode();

        #endregion

        #endregion

        #region Grow-/Shrink-/Replicate-/WipeFileSystem

        /// <summary>
        /// This enlarges the size of a GraphFS
        /// </summary>
        /// <param name="myNumberOfBytesToAdd">the number of bytes to add to the size of the current file system</param>
        /// <returns>New total number of bytes</returns>
        UInt64 GrowFileSystem(UInt64 myNumberOfBytesToAdd);

        /// <summary>
        /// This reduces the size of a GraphFS
        /// </summary>
        /// <param name="myNumberOfBytesToRemove">the number of bytes to remove from the size of the current file system</param>
        /// <returns>New total number of bytes</returns>
        UInt64 ShrinkFileSystem(UInt64 myNumberOfBytesToRemove);

        /// <summary>
        /// Wipe the file system
        /// </summary>
        void WipeFileSystem();

        /// <summary>
        /// Clones the IGraphFS instance into a stream
        /// </summary>
        /// <param name="myTimeStamp">the starting timestamp of the clone. every vertex that has been created after this timestamp has to be returned</param>
        /// <returns>An enumerable of to be cloned vertices</returns>
        IEnumerable<IVertex> CloneFileSystem(UInt64 myTimeStamp = 0UL);

        /// <summary>
        /// Initializes a GraphFS using the replicated vertices
        /// </summary>
        /// <param name="myReplicationStream">An enumerable of vertices</param>
        void ReplicateFileSystem(IEnumerable<IVertex> myReplicationStream);

        #endregion

        #region Mount-/Remount-/UnmountFileSystem

        /// <summary>
        /// Mounts this file system.
        /// </summary>
        /// <param name="myAccessMode">The file system access mode, e.g. "read-write" or "read-only".</param>
        void MountFileSystem(FileSystemAccessMode myAccessMode);

        /// <summary>
        /// Remounts a file system in order to change its access mode.
        /// </summary>
        /// <param name="myFSAccessMode">The file system access mode, e.g. "read-write" or "read-only".</param>
        void RemountFileSystem(FileSystemAccessMode myFSAccessMode);

        /// <summary>
        /// Flush all caches and unmount this file system.
        /// </summary>
        void UnmountFileSystem();

        #endregion

        #region Vertex

        /// <summary>
        /// Checks if a vertex exists
        /// </summary>
        /// <param name="myVertexID">The id of the vertex</param>
        /// <param name="myVertexTypeID">The id of the vertex type</param>
        /// <param name="myEdition">The edition of the vertex  (if left out, the default edition is assumed)</param>
        /// <param name="myVertexRevisionID">The revision id if the vertex (if left out, the latest revision is assumed)</param>
        /// <returns>True if the vertex exists, otherwise false</returns>
        Boolean VertexExists(
            UInt64 myVertexID,
            UInt64 myVertexTypeID,
            String myEdition = null,
            VertexRevisionID myVertexRevisionID = null);

        /// <summary>
        /// Gets a vertex 
        /// If there is no edition or revision given, the default edition and the latest revision is returned
        /// </summary>
        /// <param name="myVertexID">The id of the vertex</param>
        /// <param name="myVertexTypeID">The id of the vertex type</param>
        /// <param name="myEdition">The edition of the vertex (if left out, the default edition is returned)</param>
        /// <param name="myVertexRevisionID">The revision id if the vertex (if left out, the latest revision is returned)</param>
        /// <returns>A vertex object or null if there is no such vertex</returns>
        IVertex GetVertex(
            UInt64 myVertexID,
            UInt64 myVertexTypeID,
            String myEdition = null,
            VertexRevisionID myVertexRevisionID = null);

        /// <summary>
        /// Returns all vertices.
        /// It is possible to filter the vertex type and the vertices itself
        /// </summary>
        /// <param name="myInterestingVertexTypeIDs">Interesting vertex type ids</param>
        /// <param name="myInterestingVertexIDs">Interesting vertex ids</param>
        /// <param name="myInterestingEditionNames">Interesting editions of the vertex</param>
        /// <param name="myInterestingRevisionIDs">Interesting revisions of the vertex</param>
        /// <returns>An IEnumerable of vertices</returns>
        IEnumerable<IVertex> GetAllVertices(
            IEnumerable<UInt64> myInterestingVertexTypeIDs = null,
            IEnumerable<UInt64> myInterestingVertexIDs = null,
            IEnumerable<String> myInterestingEditionNames = null,
            IEnumerable<VertexRevisionID> myInterestingRevisionIDs = null);

        /// <summary>
        /// Returns all vertex by a given typeID. It's possible to filter interesting vertices.
        /// Edition and Revision filtering works by using a filter func.
        /// 
        /// Beware: defining funcs means that the function has to be called on any vertex.
        /// If you know the exact definitions use the overloaded version of this method using
        /// IEnumerable instead.
        /// </summary>
        /// <param name="myTypeID">the considered vertex type</param>
        /// <param name="myInterestingVertexIDs">a set of vertexID which shall be loaded</param>
        /// <param name="myEditionsFilterFunc">func to filter editions</param>
        /// <param name="myInterestingRevisionIDFilterFunc">func to filter revisions</param>
        /// <returns>vertices</returns>
        IEnumerable<IVertex> GetVerticesByTypeID(
            UInt64 myTypeID,
            IEnumerable<UInt64> myInterestingVertexIDs = null,
            Func<String, bool> myEditionsFilterFunc = null,
            Func<VertexRevisionID, bool> myInterestingRevisionIDFilterFunc = null);

        /// <summary>
        /// Returns all vertex by a given typeID. It's possible to filter interesting vertices.
        /// It's also possible to filter specified vertices by id, their editions and revisions.
        /// </summary>
        /// <param name="myTypeID">the considered vertex type</param>
        /// <param name="myInterestingVertexIDs">a set of vertexID which shall be loaded</param>
        /// <param name="myInterestingEditionNames">a set of interesting editions of a vertex</param>
        /// <param name="myInterestingRevisionIDs">a set of interesting revisions of a vertex</param>
        /// <returns>vertices</returns>
        IEnumerable<IVertex> GetVerticesByTypeID(
            UInt64 myTypeID,
            IEnumerable<UInt64> myInterestingVertexIDs = null,
            IEnumerable<String> myInterestingEditionNames = null,
            IEnumerable<VertexRevisionID> myInterestingRevisionIDs = null);

        /// <summary>
        /// Returns all vertices considering a given vertex type.
        /// 
        /// The default edition and latest revision of an existing vertex will be returned.
        /// </summary>
        /// <param name="myTypeID">the considered vertex type</param>
        /// <param name="myInterestingVertexIDs">a set of vertexID which shall be loaded</param>
        /// <returns>all interesting vertices of given type with default edition and latest revision</returns>
        IEnumerable<IVertex> GetVerticesByTypeID(
            UInt64 myTypeID,
            IEnumerable<UInt64> myInterestingVertexIDs);

        /// <summary>
        /// Returns all vertices considering a given vertex type.
        /// 
        /// The default edition of the vertex will be returned (if the defined revision exists)
        /// </summary>
        /// <param name="myTypeID">the considered vertex type</param>
        /// <param name="myInterestingRevisions">a set of (vertex-)revisions which are of interest</param>
        /// <returns>all vertices of given type which are available at the given revisions</returns>
        IEnumerable<IVertex> GetVerticesByTypeID(
            UInt64 myTypeID,
            IEnumerable<VertexRevisionID> myInterestingRevisions);

        /// <summary>
        /// Returns all editions corresponding to a certain vertex
        /// </summary>
        /// <param name="myVertexID">The id of the vertex</param>
        /// <param name="myVertexTypeID">The id of the vertex type</param>
        /// <returns>An IEnumerable of editions</returns>
        IEnumerable<String> GetVertexEditions(
            UInt64 myVertexID,
            UInt64 myVertexTypeID);

        /// <summary>
        /// Returns all revision ids to a certain vertex and edition
        /// </summary>
        /// <param name="myVertexID">The id of the vertex</param>
        /// <param name="myVertexTypeID">The id of the vertex type</param>
        /// <param name="myInterestingEditions">The interesting vertex editions</param>
        /// <returns>An IEnumerable of VertexRevisionIDs</returns>
        IEnumerable<VertexRevisionID> GetVertexRevisionIDs(
            UInt64 myVertexID,
            UInt64 myVertexTypeID,
            IEnumerable<String> myInterestingEditions = null);

        /// <summary>
        /// Removes a certain revision of a vertex
        /// </summary>
        /// <param name="myVertexID">The id of the vertex</param>
        /// <param name="myVertexTypeID">The id of the vertex type</param>
        /// <param name="myInterestingEditions">The interesting editions of the vertex</param>
        /// <param name="myToBeRemovedRevisionIDs">The revisions that should be removed</param>
        /// <returns>True if some revisions have been removed, false otherwise</returns>
        bool RemoveVertexRevision(
            UInt64 myVertexID,
            UInt64 myVertexTypeID,
            IEnumerable<String> myInterestingEditions = null,
            IEnumerable<VertexRevisionID> myToBeRemovedRevisionIDs = null);

        /// <summary>
        /// Removes a certain edition of a vertex
        /// </summary>
        /// <param name="myVertexID">The id of the vertex</param>
        /// <param name="myVertexTypeID">The id of the vertex type</param>
        /// <param name="myToBeRemovedEditions">The editions that should be removed</param>
        /// <returns>True if some revisions have been removed, false otherwise</returns>
        bool RemoveVertexEdition(
            UInt64 myVertexID,
            UInt64 myVertexTypeID,
            IEnumerable<String> myToBeRemovedEditions = null);

        /// <summary>
        /// Removes a vertex entirely
        /// </summary>
        /// <param name="myVertexID">The id of the vertex</param>
        /// <param name="myVertexTypeID">The id of the vertex type</param>
        /// <returns>True if a vertex has been erased, false otherwise</returns>
        bool RemoveVertex(
            UInt64 myVertexID,
            UInt64 myVertexTypeID);

        /// <summary>
        /// Adds a new vertex to the graph fs and returns it
        /// </summary>
        /// <param name="myVertex">The vertex that is going to be inserted</param>
        /// <param name="myEdition">The name of the edition of the new vertex</param>
        /// <param name="myVertexRevisionID">The revision id of the vertex</param>
        bool AddVertex(
            IVertex myVertex,
            String myEdition = null,
            VertexRevisionID myVertexRevisionID = null);

        /// <summary>
        /// Updates a vertex and returns it
        /// </summary>
        /// <param name="myToBeUpdatedVertexID">The vertex id that is going to be updated</param>
        /// <param name="myCorrespondingVertexTypeID">The vertex type id that is going to be updated</param>
        /// <param name="myVertexUpdateDiff">The update for the vertex</param>
        /// <param name="myToBeUpdatedEditions">The editions that should be updated</param>
        /// <param name="myToBeUpdatedRevisionIDs">The revisions that should be updated</param>
        /// <param name="myCreateNewRevision">Determines if it is necessary to create a new revision of the vertex</param>
        IVertex UpdateVertex(
            UInt64 myToBeUpdatedVertexID,
            UInt64 myCorrespondingVertexTypeID,
            IVertex myVertexUpdateDiff,
            String myToBeUpdatedEditions = null,
            VertexRevisionID myToBeUpdatedRevisionIDs = null,
            Boolean myCreateNewRevision = false);

        #endregion
    }
}